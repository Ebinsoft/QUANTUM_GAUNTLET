using UnityEngine;

public abstract class PlayerBaseState
{
    private PlayerManager player;

    // Behavior booleans - concrete states can override these to easily modify common behaviors
    // allows X/Z movement during state
    protected bool canMove = false;
    protected bool canRotate = false;
    protected bool cancelMomentum = false;
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
        player.currentState.Cleanup();
        player.currentState = newState;
        // if (player.gameObject.name == "DummyPlayer")
        // {
        // Debug.Log(player.currentState);
        // }
        player.currentState.Setup();
        player.currentState.EnterState();
    }
    public void Update()
    {

        if (player.currentState.canMove && player.isMovePressed)
        {
            Move();
        }

        if (player.currentState.canRotate && player.isMovePressed)
        {
            Rotate();
        }

        // not sure if this is best place to put this, ask tyler
        player.anim.SetBool("IsGrounded", player.characterController.isGrounded);
        player.currentState.anyStateUpdate();
        player.currentState.UpdateState();
    }

    private void Move()
    {
        player.currentMovement.x = player.playerSpeed * player.inputMovement.x;
        player.currentMovement.z = player.playerSpeed * player.inputMovement.y;

        player.anim.SetFloat("MoveSpeed", player.inputMovement.magnitude);
    }

    private void Rotate()
    {
        player.rotationTarget.x = player.inputMovement.x;
        player.rotationTarget.y = player.inputMovement.y;
    }

    // High priority state transitions that all states share.
    private void anyStateUpdate()
    {
        if (player.triggerDead)
        {
            player.triggerHit = false;
            SwitchState(player.DeadState);
        }
        else if (player.triggerHit)
        {
            if (player.characterController.isGrounded)
            {
                SwitchState(player.StunState);
            }

            else
            {
                SwitchState(player.TumblingState);
            }

        }

        else
        {
            CheckStateUpdate();
        }
    }

    private void Setup()
    {
        if (player.currentState.cancelMomentum)
        {
            EndMomentum();
        }
    }

    private void Cleanup()
    {
        if (player.currentState.cancelMomentum)
        {
            EndMomentum();
        }
    }

    private void EndMomentum()
    {
        player.isMoving = false;
        player.currentMovement.x = 0.0f;
        player.currentMovement.z = 0.0f;
        player.anim.SetFloat("MoveSpeed", 0);
    }

}
