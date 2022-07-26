using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawningState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerRespawningState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;

    }

    public override void EnterState()
    {
        player.isRespawning = true;
        player.animEffects.CancelHit();

        player.stats.ResetStats();
        player.stats.currentStatus = PlayerStats.Status.normal;

        player.anim.Play("Idle");
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isRespawning = false;
        player.canDie = true;
    }

    public override void CheckStateUpdate()
    {
        if (!player.anim.GetBool("InRespawn"))
        {
            SwitchState(player.IdleState);
        }
    }
}
