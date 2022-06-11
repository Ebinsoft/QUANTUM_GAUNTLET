using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerManager player;
    private float dashTimer = 0.0f;
    private float dashDuration = 2.0f;
    public PlayerDashState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }
    public override void EnterState()
    {
        player.isDashing = true;
        // just a guess for Jesse
        // player.anim.SetTrigger("Dash");
        dashTimer = 0.0f;
    }

    public override void UpdateState()
    {
        dashTimer += Time.deltaTime;
    }

    public override void ExitState()
    {
        player.isDashing = false;

    }

    public override void CheckStateUpdate()
    {
        // also a guess for Jesse
        // if (!player.anim.GetBool("InDash"))
        if (dashTimer >= dashDuration)
        {
            SwitchState(player.IdleState);
            player.dashesLeft = player.maxDashes;
        }

        else if (player.dashesLeft > 0 && player.isUtilityAttackTriggered)
        {
            SwitchState(player.JumpingState);
        }
    }

    private void Dash()
    {
        player.currentMovement.y = player.initialJumpVelocity;
        player.dashesLeft--;
        player.isUtilityAttackTriggered = false;
    }
}
