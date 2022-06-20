using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private PlayerManager player;

    public PlayerHitState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }

    public override void EnterState()
    {
        player.isHit = true;

        player.triggerHit = false;

        // force the animator to ignore its current transition rules and play the stun animation
        player.anim.Play("Take Hit");
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isHit = false;
    }

    public override void CheckStateUpdate()
    {
        if (player.isStunned) return;

        if (!player.anim.GetBool("InHit") && player.characterController.isGrounded)
        {
            SwitchState(player.IdleState);
            player.playerKnockback.StopKnockback();
        }

        else if (!player.anim.GetBool("InHit") && !player.characterController.isGrounded)
        {
            SwitchState(player.FallingState);
        }
    }
}
