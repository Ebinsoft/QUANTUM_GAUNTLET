using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVictoryState : PlayerBaseState
{
    private PlayerManager player;

    public PlayerVictoryState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState()
    {
        player.anim.Play("Victory");
    }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void CheckStateUpdate()
    {
        // not sure if we ever want to transition out of this state?
    }
}
