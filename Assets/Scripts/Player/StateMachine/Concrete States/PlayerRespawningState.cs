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
        player.stats.PlayerRespawn();


        player.stats.ResetStats();
        // do animation stuff
        // foreach (var param in player.anim.parameters)
        // {
        //     if (param.type == AnimatorControllerParameterType.Trigger)
        //     {
        //         player.anim.ResetTrigger(param.name);
        //     }
        // }
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
