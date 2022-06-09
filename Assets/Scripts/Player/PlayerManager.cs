using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public PlayerBaseState currentState;

    // One for each concrete state
    public PlayerIdleState IdleState;
    public PlayerWalkingState WalkingState;
    public PlayerJumpingState JumpingState;
    public PlayerFallingState FallingState;
    public PlayerLandingState LandingState;
    public PlayerNormalAttackState NormalAttackState;

    // Other Stuff
    public Animator anim;
    private PlayerInput playerInput;
    public CharacterController characterController;

    // Handles all movmement once per frame with cc.Move
    public Vector3 currentMovement;

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

    /***************************************/
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

    // gravity variables
    private float gravity;
    public float gravityMultiplier = 1.0f;
    private float groundedGravity = -0.05f;
    public float maxFallingSpeed = -15.0f;

    // normal-attack variables
    public bool isAttacking = false;
    public int maxAttackChain = 3;
    public int attacksLeft = 3;

    void Awake()
    {
        // initialize each concrete state
        IdleState = new PlayerIdleState(this);
        WalkingState = new PlayerWalkingState(this);
        JumpingState = new PlayerJumpingState(this);
        FallingState = new PlayerFallingState(this);
        LandingState = new PlayerLandingState(this);
        NormalAttackState = new PlayerNormalAttackState(this);

        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();

        currentMovement = new Vector3(0.0f, 0.0f, 0.0f);
        setupJumpVariables();
        jumpsLeft = maxJumps;
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
        handleRotation();
        currentState.Update();

        characterController.Move(currentMovement * Time.deltaTime);
    }

    void FixedUpdate()
    {
        resetJumps();
        handleGravity();
    }

    void setupJumpVariables()
    {
        // setting gravity and our jump velocity in terms of jump height and jump time
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        // rotation
        if (positionToLookAt.magnitude > 0)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void handleGravity()
    {
        // this will handle early falling if you release the jump button
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        // a lower grounded gravity makes clipping less likely but will still trigger isGrounded
        if (characterController.isGrounded)
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
        if (characterController.isGrounded && jumpsLeft < maxJumps)
        {
            jumpsLeft = maxJumps;
        }
    }

}
