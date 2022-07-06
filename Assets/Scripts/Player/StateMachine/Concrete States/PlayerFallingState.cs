using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerFallingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = true;
        canRotate = true;
        cancelMomentum = true;
    }
    public override void EnterState()
    {
        player.isFalling = true;
        player.anim.SetBool("IsFalling", true);
        player.gravityMultiplier = player.fallMultiplier;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isFalling = false;
        player.anim.SetBool("IsFalling", false);
        player.gravityMultiplier = 1.0f;
    }

    public override void CheckStateUpdate()
    {
        if (player.isGrounded)
        {
            SwitchState(player.LandingState);
        }
        else if (player.jumpsLeft > 0 && player.isJumpTriggered)
        {
            SwitchState(player.JumpingState);
        }
    }
}
