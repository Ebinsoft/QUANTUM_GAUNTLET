using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{

    private PlayerStateManager player;
    public PlayerJumpingState(PlayerStateManager psm) : base(psm)
    {
        player = psm;
        canMove = true;
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
        if (player.characterController.isGrounded)
        {
            Debug.Log("doodoo");
            SwitchState(player.LandingState);
        }

        else if (player.currentMovement.y < 0.0f)
        {
            SwitchState(player.FallingState);
        }

        else if (player.jumpsLeft > 0 && player.canJump)
        {
            SwitchState(player.JumpingState);
        }
    }

    private void Jump()
    {
        player.currentMovement.y = player.initialJumpVelocity;
        player.jumpsLeft--;
        player.canJump = false;
    }
}
