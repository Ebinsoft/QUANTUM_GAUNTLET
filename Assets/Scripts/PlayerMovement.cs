using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Rigidbody rb;

    // movement variables
    private Vector3 currentMovement;
    public float playerSpeed = 3.5f;

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

    private void Awake() {
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
        bool isFalling = currentMovement.y <= 0.0f || !isJumpPressed;
        // a lower grounded gravity makes clipping less likely but will still trigger isGrounded
        if(characterController.isGrounded) {
            currentMovement.y = groundedGravity;
        }
        else {
            float multiplier = isFalling ? fallMultiplier : 1.0f;
            currentMovement.y += Mathf.Max((gravity * multiplier * Time.deltaTime), gravity);
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



    private void onJump(InputAction.CallbackContext context) {
        isJumpPressed = context.ReadValueAsButton();
        if(jumpsLeft > 0 && isJumpPressed == true) {
            currentMovement.y = initialJumpVelocity;
            jumpsLeft--;
        }
    }

    private void resetJumps() {
        if(characterController.isGrounded && jumpsLeft < maxJumps) {
            jumpsLeft = maxJumps;
        }
        // Steals a jump if you're not on the ground(like smash). This part is finicky since we're 
        // relying on isGrounded. Should probably find a better way or remove the jump stealing.
        else if(!characterController.isGrounded && jumpsLeft == maxJumps) {
            //jumpsLeft = maxJumps-1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentMovement = new Vector3 (0.0f, currentMovement.y, 0.0f);

        if(playerInput.Player.Move.inProgress) {
            Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();
            currentMovement += new Vector3(playerSpeed * move.x, 0.0f, playerSpeed * move.y);
        }
        characterController.Move(currentMovement * Time.deltaTime);
        
    }

    void FixedUpdate() 
    {
        resetJumps();
        handleGravity();
    }

}
