using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStunState : PlayerBaseState
{
    private PlayerManager player;

    public PlayerStunState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = false;
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
        if (!player.characterController.isGrounded)
        {
            SwitchState(player.TumblingState);
        }

        else if (!player.isStunned)
        {
            player.playerKnockback.StopKnockback();
            SwitchState(player.IdleState);
        }

    }
}
