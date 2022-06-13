using UnityEngine;

public abstract class PlayerBaseState
{
    private PlayerManager player;

    // Behavior booleans - concrete states can override these to easily modify common behaviors
    // allows X/Z movement during state
    protected bool canMove = false;
    public PlayerBaseState(PlayerManager psm)
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

        anyStateUpdate();
        UpdateState();
    }

    private void Move()
    {
        player.currentMovement.x = player.playerSpeed * player.inputMovement.x;
        player.currentMovement.z = player.playerSpeed * player.inputMovement.y;

        player.anim.SetFloat("MoveSpeed", player.inputMovement.magnitude);
    }

    // High priority state transitions that all states share.
    private void anyStateUpdate()
    {
        if (player.triggerHit)
        {
            SwitchState(player.HitState);
        }

        else
        {
            CheckStateUpdate();
        }
    }

    private void Setup()
    {

    }

    private void Cleanup()
    {
        if (canMove)
        {
            player.isMoving = false;
            player.currentMovement.x = 0.0f;
            player.currentMovement.z = 0.0f;
            player.anim.SetFloat("MoveSpeed", 0);
        }
    }

}
