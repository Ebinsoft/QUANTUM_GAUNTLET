using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDyingState : PlayerBaseState
{
    private PlayerManager player;

    public PlayerDyingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        player.anim.Play("Die");
        player.anim.SetBool("InDying", true);

        player.playerKnockback.StopKnockback();
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void CheckStateUpdate()
    {
        if (!player.anim.GetBool("InDying"))
        {
            SwitchState(player.DeadState);
        }
    }
}
