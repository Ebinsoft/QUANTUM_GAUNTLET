using UnityEngine;

public class PlayerDashingState : PlayerBaseState
{
    public PlayerManager player;
    private float dashTimer = 0.0f;
    private float dashDuration = 0.15f;
    public PlayerDashingState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }
    public override void EnterState()
    {
        player.isDashing = true;
        // just a guess for Jesse
        // player.anim.SetTrigger("Dash");
        dashTimer = 0.0f;
        Dash();
    }

    public override void UpdateState()
    {
        dashTimer += Time.deltaTime;
    }

    public override void ExitState()
    {
        player.isDashing = false;
        player.currentMovement.x = 0;
        player.currentMovement.z = 0;

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
            SwitchState(player.DashingState);
        }
    }

    private void Dash()
    {
        Vector3 playerFacing = player.transform.forward;
        player.currentMovement = (playerFacing * player.initialDashVelocity);
        player.dashesLeft--;
        player.isUtilityAttackTriggered = false;
    }
}
