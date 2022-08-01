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
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
    }

    private void onMove(InputAction.CallbackContext context)
    {
        player.inputMovement = playerInput.actions["Move"].ReadValue<Vector2>();
        player.isMovePressed = player.inputMovement.magnitude > 0;
    }

    private void onJump(InputAction.CallbackContext context)
    {
        player.isJumpPressed = context.ReadValueAsButton();
        player.isJumpTriggered = context.ReadValueAsButton();

    }

    private void onLightAttack(InputAction.CallbackContext context)
    {
        player.isLightAttackPressed = context.ReadValueAsButton();
        player.isLightAttackTriggered = context.ReadValueAsButton();
    }

    private void onHeavyAttack(InputAction.CallbackContext context)
    {
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

    private void onPowerToggle(InputAction.CallbackContext context)
    {
        player.isPowerTogglePressed = context.ReadValueAsButton();
    }



    private void OnEnable()
    {

        // subscribe to events

        playerInput.actions["Move"].started += onMove;
        playerInput.actions["Move"].performed += onMove;
        playerInput.actions["Move"].canceled += onMove;

        playerInput.actions["Jump"].started += onJump;
        playerInput.actions["Jump"].canceled += onJump;

        playerInput.actions["LightAttack"].started += onLightAttack;
        playerInput.actions["LightAttack"].canceled += onLightAttack;

        playerInput.actions["HeavyAttack"].started += onHeavyAttack;
        playerInput.actions["HeavyAttack"].canceled += onHeavyAttack;

        playerInput.actions["UtilityAttack"].started += onUtilityAttack;
        playerInput.actions["UtilityAttack"].canceled += onUtilityAttack;

        playerInput.actions["Special1"].started += onSpecial1;
        playerInput.actions["Special1"].canceled += onSpecial1;

        playerInput.actions["Special2"].started += onSpecial2;
        playerInput.actions["Special2"].canceled += onSpecial2;

        playerInput.actions["Special3"].started += onSpecial3;
        playerInput.actions["Special3"].canceled += onSpecial3;

        playerInput.actions["Block"].started += onBlock;
        playerInput.actions["Block"].canceled += onBlock;

        playerInput.actions["Start"].started += onStart;
        playerInput.actions["Start"].canceled += onStart;

        playerInput.actions["Select"].started += onSelect;
        playerInput.actions["Select"].canceled += onSelect;

        playerInput.actions["PowerToggle"].started += onPowerToggle;
        playerInput.actions["PowerToggle"].canceled += onPowerToggle;

    }

    private void OnDisable()
    {

        // unsubscribe to events

        playerInput.actions["Move"].started -= onMove;
        playerInput.actions["Move"].performed -= onMove;
        playerInput.actions["Move"].canceled -= onMove;

        playerInput.actions["Jump"].started -= onJump;
        playerInput.actions["Jump"].canceled -= onJump;

        playerInput.actions["LightAttack"].started -= onLightAttack;
        playerInput.actions["LightAttack"].canceled -= onLightAttack;

        playerInput.actions["HeavyAttack"].started -= onHeavyAttack;
        playerInput.actions["HeavyAttack"].canceled -= onHeavyAttack;

        playerInput.actions["UtilityAttack"].started -= onUtilityAttack;
        playerInput.actions["UtilityAttack"].canceled -= onUtilityAttack;

        playerInput.actions["Special1"].started -= onSpecial1;
        playerInput.actions["Special1"].canceled -= onSpecial1;

        playerInput.actions["Special2"].started -= onSpecial2;
        playerInput.actions["Special2"].canceled -= onSpecial2;

        playerInput.actions["Special3"].started -= onSpecial3;
        playerInput.actions["Special3"].canceled -= onSpecial3;

        playerInput.actions["Block"].started -= onBlock;
        playerInput.actions["Block"].canceled -= onBlock;

        playerInput.actions["Start"].started -= onStart;
        playerInput.actions["Start"].canceled -= onStart;

        playerInput.actions["Select"].started -= onSelect;
        playerInput.actions["Select"].canceled -= onSelect;

        playerInput.actions["PowerToggle"].started -= onPowerToggle;
        playerInput.actions["PowerToggle"].started -= onPowerToggle;
    }
}
