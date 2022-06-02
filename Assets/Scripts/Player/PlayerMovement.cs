using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerManager playerManager;
    private PlayerInput playerInput;
    // movement variables
    public float playerSpeed = 3.5f;

    void Awake() 
    {
        playerManager = GetComponent<PlayerManager>();
        playerInput = new PlayerInput();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Move.started += onMove;
        playerInput.Player.Move.performed += onMove;
        playerInput.Player.Move.canceled += onMove;

    }

    private void onMove(InputAction.CallbackContext context)
    {
        if(playerInput.Player.Move.inProgress) {
            
            Vector2 move = playerInput.Player.Move.ReadValue<Vector2>();
            playerManager.currentMovement.x = playerSpeed * move.x;
            playerManager.currentMovement.z = playerSpeed * move.y;
        }
        else {
            playerManager.currentMovement.x = 0.0f;
            playerManager.currentMovement.z = 0.0f;
        }
    }

    private void OnDisable()
    {
        playerInput.Disable();
        playerInput.Player.Move.started -= onMove;
        playerInput.Player.Move.performed -= onMove;
        playerInput.Player.Move.canceled -= onMove;

    }

    // Update is called once per frame
    void Update()
    {

    }
}
