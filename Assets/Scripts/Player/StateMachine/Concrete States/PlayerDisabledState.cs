using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A generic class for disabling players
public class PlayerDisabledState : PlayerBaseState
{
    private PlayerManager player;

    public PlayerDisabledState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }

    public override void EnterState() { }

    public override void UpdateState() { }

    public override void ExitState() { }

    public override void CheckStateUpdate()
    {
        // not sure if we ever want to transition out of this state?
    }
}
