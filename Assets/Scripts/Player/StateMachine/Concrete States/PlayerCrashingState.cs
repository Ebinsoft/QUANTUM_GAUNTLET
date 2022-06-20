using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrashingState : PlayerBaseState
{
    private PlayerManager player;
    private float maximumCrashTime = 2.0f;
    private float crashTimer = 0.0f;

    public PlayerCrashingState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }
    public override void EnterState()
    {
        player.isCrashing = true;
        crashTimer = 0.0f;
        player.stats.currentStatus = PlayerStats.Status.intangible;
    }

    public override void UpdateState()
    {
        crashTimer += Time.deltaTime;
    }

    public override void ExitState()
    {
        player.isCrashing = false;
        player.stats.currentStatus = PlayerStats.Status.normal;
    }

    public override void CheckStateUpdate()
    {
        // Add animation triggering
        if (crashTimer >= maximumCrashTime)
        {
            SwitchState(player.IdleState);
        }
    }
}