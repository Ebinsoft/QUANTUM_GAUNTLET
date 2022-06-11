using UnityEngine;

public class PlayerDashingState : PlayerBaseState
{
    public PlayerManager player;
    private PlayerParticleEffects effects;
    private float dashTimer = 0.0f;
    private float dashDuration = 0.15f;
    public PlayerDashingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        effects = player.gameObject.GetComponent<PlayerParticleEffects>();
    }
    public override void EnterState()
    {
        player.isDashing = true;
        player.anim.SetBool("IsDashing", true);
        effects.StartDashingEffect();
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
        player.anim.SetBool("IsDashing", false);
        effects.StopDashingEffect();
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
