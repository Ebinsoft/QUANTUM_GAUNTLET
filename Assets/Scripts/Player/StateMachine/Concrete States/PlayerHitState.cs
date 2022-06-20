using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private PlayerManager player;
    private HitData hitAttack;

    private float startTime;
    private float stunTime;
    private bool knockbackTriggered;

    public PlayerHitState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }

    public override void EnterState()
    {
        player.isHit = true;

        // force the animator to ignore its current transition rules and play the stun animation
        player.anim.Play("Take Hit");

        // pull out hitData and reset the trigger
        hitAttack = player.triggerHit.Value;
        player.triggerHit = null;

        // pull out stun time for easier access
        stunTime = hitAttack.attack.stunTime;

        knockbackTriggered = false;
    }


    public override void UpdateState()
    {
        if (!player.isHitLagging)
        {
            // this whole block should only run once after hitlag stops
            if (stunTime > 0 && !knockbackTriggered)
            {
                player.playerKnockback.ApplyKnockback(hitAttack);

                knockbackTriggered = true;
            }
        }
    }

    public override void ExitState()
    {
        player.isHit = false;
    }

    public override void CheckStateUpdate()
    {
        if (Time.time - startTime < stunTime) return;

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
