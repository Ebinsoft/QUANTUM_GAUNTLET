using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{

    private PlayerManager player;
    public PlayerJumpingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = true;
        canRotate = true;
        cancelMomentum = true;
    }
    public override void EnterState()
    {
        player.isJumping = true;
        player.anim.SetTrigger("Jump");
        Jump();
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isJumping = false;
    }

    public override void CheckStateUpdate()
    {
        if (player.isGrounded)
        {
            SwitchState(player.LandingState);
        }

        else if (player.currentMovement.y < 0.0f)
        {
            SwitchState(player.FallingState);
        }

        else if (player.jumpsLeft > 0 && player.isJumpTriggered)
        {
            SwitchState(player.JumpingState);
        }
    }

    private void Jump()
    {
        player.gravity = player.jumpGravity;
        player.currentMovement.y = player.initialJumpVelocity;
        player.jumpsLeft--;
        player.isJumpTriggered = false;
    }
}
