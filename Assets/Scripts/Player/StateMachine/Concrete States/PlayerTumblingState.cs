using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTumblingState : PlayerBaseState
{
    private PlayerManager player;

    public PlayerTumblingState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }

    public override void EnterState()
    {
        player.isTumbling = true;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isTumbling = false;
    }

    public override void CheckStateUpdate()
    {
        if (!player.isStunned)
        {
            SwitchState(player.FallingState);
        }

        else if (player.characterController.isGrounded)
        {
            player.playerKnockback.StopKnockback();
            SwitchState(player.CrashingState);
        }
    }
}
