using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTumblingState : PlayerBaseState
{
    private PlayerManager player;
    private PlayerParticleEffects effects;

    public PlayerTumblingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = false;

        effects = player.gameObject.GetComponent<PlayerParticleEffects>();
    }

    public override void EnterState()
    {
        player.isTumbling = true;
        effects.StartDashingEffect();
    }

    public override void UpdateState()
    {
        if (player.currentMovement.y < 0)
        {
            effects.StopDashingEffect();
        }
    }

    public override void ExitState()
    {
        player.isTumbling = false;
        effects.StopDashingEffect();
    }

    public override void CheckStateUpdate()
    {
        if (!player.isStunned)
        {
            player.playerKnockback.StopKnockback();
            SwitchState(player.AirborneState);
        }

        else if (player.isGrounded)
        {
            player.playerKnockback.StopKnockback();
            SwitchState(player.CrashingState);
        }
    }
}
