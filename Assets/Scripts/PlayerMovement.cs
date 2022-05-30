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
    private float jumpsLeft;


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
            currentMovement.y = groundedGravity;
        }
        else {
            currentMovement.y = gravity * Time.deltaTime;
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
        if(characterController.isGrounded && jumpsLeft < maxJumps) {
            jumpsLeft = maxJumps;
        }

        if(jumpsLeft > 0) {
            characterController.Move(new Vector3(0.0f, jumpForce, 0.0f));
            jumpsLeft -= 1;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        currentMovement = new Vector3(0.0f, 0.0f, 0.0f);
        if(playerInput.Player.Move.inProgress) {
            Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();
            currentMovement = new Vector3(playerSpeed * Time.deltaTime * move.x, 0.0f, playerSpeed * Time.deltaTime * move.y);
            
        }
        handleGravity();
        characterController.Move(currentMovement);
    }

}
