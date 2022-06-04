using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    public PlayerBaseState currentState;

    public PlayerManager playerManager;

    // One for each concrete state
    public PlayerIdleState IdleState;
    public PlayerMovingState MovingState;
    public PlayerJumpingState JumpingState;
    public PlayerFallingState FallingState;

    // Other Stuff
    public Animator anim;
    private PlayerInput playerInput;
    public CharacterController characterController;

    // Animation flags

    // input variables
    public bool isJumpPressed = false;
    public bool isMovePressed = false;

    // movement variables
    public float playerSpeed = 7.0f;
    public float rotationSpeed = 30.0f;

    // jumping variables
    public bool isJumping = false;
    public float maxJumps = 2;
    public float maxJumpHeight = 1.0f;
    public float maxJumpTime = 0.5f;
    public float fallMultiplier = 2.0f;
    public float jumpsLeft;
    public float initialJumpVelocity;

    // gravity variables
    private float gravity;
    private float groundedGravity = -0.05f;
    public float maxFallingSpeed = -15.0f;

    void Awake() {
        // initialize each concrete state
        IdleState = new PlayerIdleState(this);
        MovingState = new PlayerMovingState(this);
        JumpingState = new PlayerJumpingState(this);
        FallingState = new PlayerFallingState(this);

        playerManager = GetComponent<PlayerManager>();
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();

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
        resetJumps();
        currentState.Update();
    }

    void FixedUpdate() {
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

        void handleGravity()
    {
        // this will handle early falling if you release the jump button
        bool isFalling = playerManager.currentMovement.y <= 0.0f || !isJumpPressed;
        // a lower grounded gravity makes clipping less likely but will still trigger isGrounded
        if (characterController.isGrounded)
        {
            playerManager.currentMovement.y = groundedGravity;
        }
        else
        {
            float multiplier = isFalling ? fallMultiplier : 1.0f;
            playerManager.currentMovement.y += (gravity * multiplier * Time.deltaTime);
            playerManager.currentMovement.y = Mathf.Max(playerManager.currentMovement.y, maxFallingSpeed);
        }
    }
    private void resetJumps()
    {
        if (characterController.isGrounded && jumpsLeft < maxJumps)
        {
            jumpsLeft = maxJumps;
        }
    }

    private void onJump(InputAction.CallbackContext context) {
        isJumpPressed = context.ReadValueAsButton();
        //anim.SetBool("Jumping", isJumpPressed);
    }

    private void OnEnable()
    {
        playerInput.Enable();

        // subscribe to events
        playerInput.Player.Jump.started += onJump;
        playerInput.Player.Jump.canceled += onJump;
    }

    private void OnDisable()
    {
        playerInput.Disable();

        // unsubscribe to events
        playerInput.Player.Jump.started -= onJump;
        playerInput.Player.Jump.canceled -= onJump;
    }
}
