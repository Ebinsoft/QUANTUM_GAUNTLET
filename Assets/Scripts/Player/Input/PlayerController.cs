using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private PlayerManager player;
    private PlayerInput playerInput;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponent<PlayerManager>();
        playerInput = new PlayerInput();
    }

    private void onMove(InputAction.CallbackContext context)
    {
        player.inputMovement = playerInput.Player.Move.ReadValue<Vector2>();
        player.isMovePressed = player.inputMovement.magnitude > 0;
    }

    private void onJump(InputAction.CallbackContext context)
    {
        player.isJumpPressed = context.ReadValueAsButton();
        player.jumpTriggered = context.ReadValueAsButton();

    }

    private void onLightAttack(InputAction.CallbackContext context)
    {
        player.isNormalAttackPressed = context.ReadValueAsButton();
        player.attackTriggered = context.ReadValueAsButton();
    }

    private void onStrongAttack(InputAction.CallbackContext context)
    {

    }

    private void onUtilityAttack(InputAction.CallbackContext context)
    {

    }

    private void onSpecial1(InputAction.CallbackContext context)
    {

    }

    private void onSpecial2(InputAction.CallbackContext context)
    {

    }

    private void onSpecial3(InputAction.CallbackContext context)
    {

    }

    private void onBlock(InputAction.CallbackContext context)
    {

    }

    private void onStart(InputAction.CallbackContext context)
    {

    }

    private void onSelect(InputAction.CallbackContext context)
    {

    }



    private void OnEnable()
    {
        playerInput.Enable();

        // subscribe to events

        playerInput.Player.Move.started += onMove;
        playerInput.Player.Move.performed += onMove;
        playerInput.Player.Move.canceled += onMove;

        playerInput.Player.Jump.started += onJump;
        playerInput.Player.Jump.canceled += onJump;

        playerInput.Player.LightAttack.started += onLightAttack;
        playerInput.Player.LightAttack.canceled += onLightAttack;

        playerInput.Player.StrongAttack.started += onStrongAttack;
        playerInput.Player.StrongAttack.canceled += onStrongAttack;

        playerInput.Player.UtilityAttack.started += onUtilityAttack;
        playerInput.Player.UtilityAttack.canceled += onUtilityAttack;


    }

    private void OnDisable()
    {
        playerInput.Disable();

        // unsubscribe to events

        playerInput.Player.Move.started -= onMove;
        playerInput.Player.Move.performed -= onMove;
        playerInput.Player.Move.canceled -= onMove;

        playerInput.Player.Jump.started -= onJump;
        playerInput.Player.Jump.canceled -= onJump;

        playerInput.Player.LightAttack.started -= onLightAttack;
        playerInput.Player.LightAttack.canceled -= onLightAttack;

        playerInput.Player.StrongAttack.started -= onStrongAttack;
        playerInput.Player.StrongAttack.canceled -= onStrongAttack;

        playerInput.Player.UtilityAttack.started -= onUtilityAttack;
        playerInput.Player.UtilityAttack.canceled -= onUtilityAttack;
    }
}
