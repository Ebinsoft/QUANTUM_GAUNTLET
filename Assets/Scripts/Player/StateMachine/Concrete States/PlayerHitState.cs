using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private PlayerManager player;
    private HitData hitAttack;

    public PlayerHitState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }

    public override void EnterState()
    {
        player.isHit = true;
        hitAttack = player.triggerHit.Value;
        player.triggerHit = null;

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
        if (!player.anim.GetBool("InHit") && player.characterController.isGrounded)
        {
            SwitchState(player.IdleState);
        }

        else if (!player.anim.GetBool("InHit") && !player.characterController.isGrounded)
        {
            SwitchState(player.JumpingState);
        }
    }
}
