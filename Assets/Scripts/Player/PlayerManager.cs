using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{

    // Player's stats
    public PlayerStats stats;
    public PlayerBaseState currentState;

    // One for each concrete state
    public PlayerIdleState IdleState;
    public PlayerWalkingState WalkingState;
    public PlayerJumpingState JumpingState;
    public PlayerAirborneState AirborneState;
    public PlayerLandingState LandingState;
    public PlayerDashingState DashingState;
    public PlayerLightAttackState LightAttackState;
    public PlayerHeavyAttackState HeavyAttackState;
    public PlayerAirLightAttackState AirLightAttackState;
    public PlayerAirHeavyAttackState AirHeavyAttackState;
    public PlayerSpecial1State Special1State;
    public PlayerSpecial2State Special2State;
    public PlayerSpecial3State Special3State;
    public PlayerStunState StunState;
    public PlayerDefeatState DefeatState;
    public PlayerDeadState DeadState;
    public PlayerRespawningState RespawnState;
    public PlayerTumblingState TumblingState;
    public PlayerCrashingState CrashingState;
    public PlayerVictoryState VictoryState;

    // Other Stuff
    public Animator anim;
    public AnimatorEffects animEffects;
    public PlayerStun playerStun;
    public PlayerKnockback playerKnockback;
    public PlayerMoveset moveset;
    public CharacterController characterController;
    public GameObject gameOverMenu;

    // Handles all movmement once per frame with cc.Move
    public Vector3 currentMovement;
    public bool isMovementEnabled = true;

    // Current XZ target direction to rotate towards
    public Vector2 rotationTarget;

    // Our updated version of isGrounded that checks a spherecast
    public bool isGrounded;
    private bool isGravityApplied = true;

    /********** input variables **********/
    // Left Stick
    public bool isMovePressed = false;
    // Gamepad South
    public bool isJumpPressed = false;
    public bool isJumpTriggered = false;
    // Gamepad West
    public bool isLightAttackPressed = false;
    public bool isLightAttackTriggered = false;
    // Gamepad North
    public bool isHeavyAttackPressed;
    public bool isHeavyAttackTriggered;
    // Gamepad East
    public bool isUtilityAttackPressed;
    public bool isUtilityAttackTriggered;
    // RT + West
    public bool isSpecial1Pressed;
    public bool isSpecial1Triggered;
    // RT + North
    public bool isSpecial2Pressed;
    public bool isSpecial2Triggered;
    // RT + East
    public bool isSpecial3Pressed;
    public bool isSpecial3Triggered;
    // LT
    public bool isBlockPressed;
    public bool isBlockTriggered;
    // Start
    public bool isStartPressed;
    public bool isStartTriggered;
    // Select
    public bool isSelectPressed;
    public bool isSelectTriggered;
    // RT
    public bool isPowerTogglePressed;

    /***************************************/

    // PLAYER ID - set up before instantiation
    public int playerID;

    // idle variables
    public bool isIdle = false;

    // movement variables
    public bool isMoving = false;
    public Vector2 inputMovement;
    public float playerSpeed = 7.0f;
    public float rotationSpeed = 30.0f;

    // jumping variables
    public bool isJumping = false;
    public float maxJumps = 2;
    public float maxJumpHeight = 1.0f;
    public float maxJumpTime = 0.5f;
    public float jumpsLeft;
    public float initialJumpVelocity;

    // falling variables
    public bool isFalling = false;

    public float fallMultiplier = 2.0f;

    // dashing variables
    public bool isDashing = false;
    public int maxDashes = 1;
    public float dashLength = 2.5f;
    public float dashesLeft;

    // hit variables
    public bool triggerHit = false;
    public bool isStunned = false;
    public bool isHit = false;
    public bool isHitLagging = false;

    // tumbling variables
    public bool isTumbling = false;

    // crashing variables
    public bool isCrashing = false;

    // gravity variables
    public float gravity;
    public float jumpGravity;
    public float gravityMultiplier = 1.0f;
    private float groundedGravity = -5f;
    public float maxFallingSpeed = -15.0f;

    // normal-attack variables
    public bool isAttacking = false;
    public int maxLightAttackChain = 4;
    public int lightAttacksLeft = 4;
    public int maxHeavyAttackChain = 1;
    public int heavyAttacksLeft = 1;

    // death variables
    public bool triggerDead = false;
    public bool playDeathAnimation = false;
    public bool canDie = true;
    public bool isDead = false;

    // respawn variables
    public bool isRespawning = false;

    // victory variables
    public bool triggerVictory = false;


    void Awake()
    {
        // initialize each concrete state
        IdleState = new PlayerIdleState(this);
        WalkingState = new PlayerWalkingState(this);
        JumpingState = new PlayerJumpingState(this);
        AirborneState = new PlayerAirborneState(this);
        LandingState = new PlayerLandingState(this);
        LightAttackState = new PlayerLightAttackState(this);
        HeavyAttackState = new PlayerHeavyAttackState(this);
        AirLightAttackState = new PlayerAirLightAttackState(this);
        AirHeavyAttackState = new PlayerAirHeavyAttackState(this);
        Special1State = new PlayerSpecial1State(this);
        Special2State = new PlayerSpecial2State(this);
        Special3State = new PlayerSpecial3State(this);
        DashingState = new PlayerDashingState(this);
        StunState = new PlayerStunState(this);
        DefeatState = new PlayerDefeatState(this);
        DeadState = new PlayerDeadState(this);
        RespawnState = new PlayerRespawningState(this);
        TumblingState = new PlayerTumblingState(this);
        CrashingState = new PlayerCrashingState(this);
        VictoryState = new PlayerVictoryState(this);

        characterController = GetComponent<CharacterController>();
        playerStun = GetComponent<PlayerStun>();
        playerKnockback = GetComponent<PlayerKnockback>();
        moveset = GetComponent<PlayerMoveset>();

        gameOverMenu = GameObject.Find("Canvas/GameOverMenu");

        currentMovement = new Vector3(0.0f, 0.0f, 0.0f);
        rotationTarget = new Vector2(transform.forward.x, transform.forward.z);
        setupJumpVariables();
        gravity = jumpGravity;
        jumpsLeft = maxJumps;
        dashesLeft = maxDashes;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = IdleState;

        currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        // check for if we died
        if (canDie && stats.health <= 0)
        {
            TriggerDeath();
        }

        if (isStartTriggered)
        {
            gameOverMenu.GetComponent<PauseMenu>().ToggleMenu();
            isStartTriggered = false;
        }

        // calculate our fancy isGrounded
        CalculateIsGrounded();

        // update animator's isGrounded to sync with code's
        anim.SetBool("IsGrounded", isGrounded);

        handleRotation();
        currentState.Update();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    private void CalculateIsGrounded()
    {
        RaycastHit hit;
        Vector3 p1 = transform.position + characterController.center;

        float capsuleWidth = 0.35f;
        float centerToFloor = (characterController.height / 2);

        bool isSphereHit = false;
        // cast a sphere
        if (Physics.SphereCast(p1, capsuleWidth, Vector3.down, out hit, centerToFloor, LayerMask.GetMask("Ground")))
        {
            isSphereHit = true;
        }
        // also make sure we have negative velocity so we don't "land" when we jump up
        isGrounded = isSphereHit && currentMovement.y <= 0f;
    }
    void FixedUpdate()
    {
        resetJumps();
        if (isGravityApplied)
        {
            handleGravity();
        }

    }

    public void DisableGravity()
    {
        isGravityApplied = false;
        currentMovement.y = 0;
    }

    public void EnableGravity()
    {
        isGravityApplied = true;
    }

    public void DisableMovement()
    {
        isMovementEnabled = false;
        currentMovement.x = 0;
        currentMovement.z = 0;
    }

    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    public void TriggerDeath(bool playAnimation = true)
    {
        canDie = false;
        triggerDead = true;
        playDeathAnimation = playAnimation;
    }

    void setupJumpVariables()
    {
        // setting gravity and our jump velocity in terms of jump height and jump time
        float timeToApex = maxJumpTime / 2;
        jumpGravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = rotationTarget.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = rotationTarget.y;

        if ((transform.forward - positionToLookAt.normalized).magnitude > 0.001f)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void Teleport(Vector3 newPosition)
    {
        characterController.enabled = false;
        transform.position = newPosition;
        characterController.enabled = true;
    }

    void handleGravity()
    {
        // this will handle early falling if you release the jump button
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        // a lower grounded gravity makes clipping less likely but will still trigger isGrounded
        if (isGrounded && currentMovement.y < 0)
        {
            currentMovement.y = groundedGravity;
        }
        else
        {
            currentMovement.y += (gravity * gravityMultiplier * Time.deltaTime);
            currentMovement.y = Mathf.Max(currentMovement.y, maxFallingSpeed);
        }
    }

    private void resetJumps()
    {
        if (isGrounded && jumpsLeft < maxJumps)
        {
            jumpsLeft = maxJumps;
        }
    }

    // checks whether the player currently has enough mana to cast a particular move
    public bool EnoughManaFor(MoveType moveType)
    {
        return moveset.TotalManaCost(moveType) <= stats.mana;
    }

    // check if our pushbox is standing on top of another player's
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if ((characterController.collisionFlags & CollisionFlags.Below) != 0)
            {
                float minHeightDiff = 1f;
                if (hit.gameObject.transform.position.y < gameObject.transform.position.y - minHeightDiff)
                {
                    // move us off their head
                    float slideSpeed = 10f;
                    Vector3 p1 = gameObject.transform.position;
                    Vector3 p2 = hit.gameObject.transform.position;
                    Vector3 dir3 = (p1 - p2);
                    Vector2 dir = new Vector2(dir3.x, dir3.z);


                    float maxDist = characterController.radius * 2.25f;
                    float speedMultiplier = Mathf.Clamp(
                        (maxDist - dir.magnitude) / maxDist,
                        0, 1
                    );

                    dir.Normalize();
                    currentMovement.x = dir.x * slideSpeed * speedMultiplier;
                    currentMovement.z = dir.y * slideSpeed * speedMultiplier;
                }
            }
        }
    }

    public void ResetAllAnimatorTriggers()
    {
        foreach (var param in anim.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                anim.ResetTrigger(param.name);
            }
        }
    }
}
