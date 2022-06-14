using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerBaseState
{
    private PlayerManager player;
    public PlayerDeadState(PlayerManager psm) : base(psm)
    {

    }

    public override void EnterState()
    {
        player.isDead = true;
    }

    public override void UpdateState()
    {
    }

    public override void ExitState()
    {
        player.isDead = false;
    }

    public override void CheckStateUpdate()
    {

    }
}
