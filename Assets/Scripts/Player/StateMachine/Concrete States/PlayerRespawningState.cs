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
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isRespawning = false;
    }

    public override void CheckStateUpdate()
    {

    }
}
