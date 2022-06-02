using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerJump : MonoBehaviour
{

    private PlayerManager playerManager;
    private PlayerInput playerInput;
    private CharacterController characterController;
    private Rigidbody rb;
    public Animator anim;

    // jumping variables
    public float maxJumps = 2;
    public float maxJumpHeight = 1.0f;
    public float maxJumpTime = 0.5f;
    public float fallMultiplier = 2.0f;
    private float initialJumpVelocity;
    private bool isJumpPressed = false;
    private float jumpsLeft;

    // gravity variables
    private float gravity;
    private float groundedGravity = -0.05f;
    public float maxFallingSpeed = -15.0f;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        setupJumpVariables();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpsLeft = maxJumps;
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



    private void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();

        // check if player is on the ground, if not, steal their first jump(like smash)
        if (jumpsLeft == maxJumps && !IsCloseToGround())
        {
            jumpsLeft = maxJumps - 1;
        }

        if (jumpsLeft > 0 && isJumpPressed == true)
        {
            playerManager.currentMovement.y = initialJumpVelocity;
            jumpsLeft--;
            anim.SetBool("Jumping", true);
        }
    }

    private bool IsCloseToGround()
    {
        float minJumpDistance = 0.6f;
        int layer_mask = LayerMask.GetMask("Ground");
        bool isClosetoGround = Physics.Raycast(transform.position, Vector3.down, minJumpDistance, layer_mask);
        return isClosetoGround;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.down * 0.6f));
    }

    private void resetJumps()
    {
        if (characterController.isGrounded && jumpsLeft < maxJumps)
        {
            jumpsLeft = maxJumps;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        resetJumps();
        handleGravity();
        anim.SetBool("InAir", !IsCloseToGround());
    }

}
