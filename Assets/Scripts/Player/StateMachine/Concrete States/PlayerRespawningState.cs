using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawningState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerRespawningState(PlayerManager psm) : base(psm)
    {
        player = psm;

    }

    public override void EnterState()
    {
        player.isRespawning = true;
        player.characterController.Move(new Vector3(0.0f, 5.0f, 0.0f));
        player.stats.resetStats();
        // do animation stuff
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
