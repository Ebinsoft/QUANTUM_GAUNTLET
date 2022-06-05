using UnityEngine;

public abstract class PlayerBaseState
{
    private PlayerStateManager player;

    // Behavior booleans - concrete states can override these to easily modify common behaviors
    // allows X/Z movement during state
    protected bool canMove = false;
    public PlayerBaseState(PlayerStateManager psm)
    {
        player = psm;

    }
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckStateUpdate();

    public void SwitchState(PlayerBaseState newState)
    {
        player.currentState.ExitState();
        Cleanup();
        Setup();
        player.currentState = newState;
        newState.EnterState();
    }
    public void Update()
    {

        if (canMove && player.isMovePressed)
        {
            Move();
        }

        // not sure if this is best place to put this, ask tyler
        player.anim.SetBool("IsGrounded", player.characterController.isGrounded);

        CheckStateUpdate();
        UpdateState();
    }

    private void Move()
    {
        player.playerManager.currentMovement.x = player.playerSpeed * player.inputMovement.x;
        player.playerManager.currentMovement.z = player.playerSpeed * player.inputMovement.y;

        player.anim.SetFloat("MoveSpeed", player.inputMovement.magnitude);
    }

    private void Setup()
    {

    }

    private void Cleanup()
    {
        if (canMove)
        {
            player.isMoving = false;
            player.playerManager.currentMovement.x = 0.0f;
            player.playerManager.currentMovement.z = 0.0f;
        }
    }

}
