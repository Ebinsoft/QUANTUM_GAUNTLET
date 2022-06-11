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
        player.isJumpTriggered = context.ReadValueAsButton();

    }

    private void onLightAttack(InputAction.CallbackContext context)
    {
        Debug.Log(player.transform.forward);
        player.isLightAttackPressed = context.ReadValueAsButton();
        player.isLightAttackTriggered = context.ReadValueAsButton();
    }

    private void onHeavyAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Heavy Attack");
        player.isHeavyAttackPressed = context.ReadValueAsButton();
        player.isHeavyAttackTriggered = context.ReadValueAsButton();
    }

    private void onUtilityAttack(InputAction.CallbackContext context)
    {
        player.isUtilityAttackPressed = context.ReadValueAsButton();
        player.isUtilityAttackTriggered = context.ReadValueAsButton();
    }

    private void onSpecial1(InputAction.CallbackContext context)
    {
        Debug.Log("Special 1");
        player.isSpecial1Pressed = context.ReadValueAsButton();
        player.isSpecial1Triggered = context.ReadValueAsButton();
    }

    private void onSpecial2(InputAction.CallbackContext context)
    {
        Debug.Log("Special 2");
        player.isSpecial2Pressed = context.ReadValueAsButton();
        player.isSpecial2Triggered = context.ReadValueAsButton();
    }

    private void onSpecial3(InputAction.CallbackContext context)
    {
        Debug.Log("Special 3");
        player.isSpecial3Pressed = context.ReadValueAsButton();
        player.isSpecial3Triggered = context.ReadValueAsButton();
    }

    private void onBlock(InputAction.CallbackContext context)
    {
        Debug.Log("block");
        player.isBlockPressed = context.ReadValueAsButton();
        player.isBlockTriggered = context.ReadValueAsButton();
    }

    private void onStart(InputAction.CallbackContext context)
    {
        Debug.Log("Start");
        player.isStartPressed = context.ReadValueAsButton();
        player.isStartTriggered = context.ReadValueAsButton();
    }

    private void onSelect(InputAction.CallbackContext context)
    {
        Debug.Log("Select");
        player.isSelectPressed = context.ReadValueAsButton();
        player.isSelectTriggered = context.ReadValueAsButton();
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

        playerInput.Player.HeavyAttack.started += onHeavyAttack;
        playerInput.Player.HeavyAttack.canceled += onHeavyAttack;

        playerInput.Player.UtilityAttack.started += onUtilityAttack;
        playerInput.Player.UtilityAttack.canceled += onUtilityAttack;

        playerInput.Player.Special1.started += onSpecial1;
        playerInput.Player.Special1.canceled += onSpecial1;

        playerInput.Player.Special2.started += onSpecial2;
        playerInput.Player.Special2.canceled += onSpecial2;

        playerInput.Player.Special3.started += onSpecial3;
        playerInput.Player.Special3.canceled += onSpecial3;

        playerInput.Player.Block.started += onBlock;
        playerInput.Player.Block.canceled += onBlock;

        playerInput.Player.Start.started += onStart;
        playerInput.Player.Start.canceled += onStart;

        playerInput.Player.Select.started += onSelect;
        playerInput.Player.Select.canceled += onSelect;


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

        playerInput.Player.HeavyAttack.started -= onHeavyAttack;
        playerInput.Player.HeavyAttack.canceled -= onHeavyAttack;

        playerInput.Player.UtilityAttack.started -= onUtilityAttack;
        playerInput.Player.UtilityAttack.canceled -= onUtilityAttack;

        playerInput.Player.Special1.started -= onSpecial1;
        playerInput.Player.Special1.canceled -= onSpecial1;

        playerInput.Player.Special2.started -= onSpecial2;
        playerInput.Player.Special2.canceled -= onSpecial2;

        playerInput.Player.Special3.started -= onSpecial3;
        playerInput.Player.Special3.canceled -= onSpecial3;

        playerInput.Player.Block.started -= onBlock;
        playerInput.Player.Block.canceled -= onBlock;

        playerInput.Player.Start.started -= onStart;
        playerInput.Player.Start.canceled -= onStart;

        playerInput.Player.Select.started -= onSelect;
        playerInput.Player.Select.canceled -= onSelect;
    }
}
