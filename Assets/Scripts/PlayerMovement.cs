using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{

    public float playerSpeed = 3.5f;
    public float jumpForce = 2.0f;
    private Rigidbody rb;
    private PlayerInput playerInput;

    private void Awake() {
        playerInput = new PlayerInput();
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
    }

    private void Jump(InputAction.CallbackContext context) {
        rb.MovePosition(transform.position + new Vector3(0f, jumpForce, 0f));
    }

    // Update is called once per frame
    void Update()
    {
        
        if(playerInput.Player.Move.inProgress) {
            Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();
            Vector3 velocity = new Vector3(playerSpeed * Time.deltaTime * move.x, 0f, playerSpeed * Time.deltaTime * move.y);
            transform.position += velocity;
        }
    }

}
