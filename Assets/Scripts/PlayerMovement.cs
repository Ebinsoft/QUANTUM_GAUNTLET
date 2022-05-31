using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    CharacterController characterController;

    private Vector3 currentMovement;
    private PlayerInput playerInput;
    private Rigidbody rb;
    public float playerSpeed = 3.5f;

    // jumping variables
    public float jumpForce = 2.0f;
    public float maxJumps = 2;
    [SerializeField]
    private float jumpsLeft;
    private bool isJumpPressed;


    // gravity variables
    float gravity = -4.0f;
    float groundedGravity = -0.05f;

    private void Awake() {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
    }

    void handleGravity() 
    {
        if(characterController.isGrounded) {
            currentMovement.y += groundedGravity;
        }
        else {
            currentMovement.y += gravity * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        playerInput.Enable();

        // subscribe to events
        playerInput.Player.Jump.started += Jump;
    }

    private void OnDisable()
    {
        playerInput.Disable();

        // unsubscribe to events
        playerInput.Player.Jump.started -= Jump;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jumpsLeft = maxJumps;
    }

    private void Jump(InputAction.CallbackContext context) {


        if(jumpsLeft > 0) {
            characterController.Move(new Vector3(0.0f, jumpForce, 0.0f));
            jumpsLeft -= 1;
        }
        
    }

    // if we touch the ground, we want to gain all of our jumps back
    // however, IIRC, in smash if you walk off an edge(or get knocked off)
    // you'll lose your first jump, so this will reduce max jumps by one if not grounded
    private void resetJumps() {
        if(characterController.isGrounded && jumpsLeft < maxJumps) {
            jumpsLeft = maxJumps;
        }
        else if(!characterController.isGrounded && jumpsLeft == maxJumps) {
            jumpsLeft = maxJumps-1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // reset currentMovement each frame and just combine everything into it that runs in update
        currentMovement = new Vector3(0.0f, 0.0f, 0.0f);

        // reset jumps, if you fall 
        resetJumps();

        if(playerInput.Player.Move.inProgress) {
            Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();
            currentMovement += new Vector3(playerSpeed * Time.deltaTime * move.x, 0.0f, playerSpeed * Time.deltaTime * move.y);
            
        }
        handleGravity();
        characterController.Move(currentMovement);
    }

}
