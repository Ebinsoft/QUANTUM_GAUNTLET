using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrashingState : PlayerBaseState
{
    private PlayerManager player;
    private float maximumCrashTime = 2.0f;
    private float crashTimer = 0.0f;
    private bool getUpTriggered = true;

    public PlayerCrashingState(PlayerManager psm) : base(psm)
    {
        player = psm;
        canMove = false;
        canRotate = false;
        cancelMomentum = true;
    }
    public override void EnterState()
    {
        player.isCrashing = true;
        crashTimer = 0.0f;
        getUpTriggered = false;
        player.stats.currentStatus = PlayerStats.Status.intangible;
    }

    public override void UpdateState()
    {
        crashTimer += Time.deltaTime;

        // getting up from crashed state can happen early if player tries to move or jump
        // otherwise it will happen automatically after 2 seconds
        if (!getUpTriggered)
        {
            if (player.isMovePressed || player.isJumpPressed || crashTimer >= maximumCrashTime)
            {
                player.anim.SetTrigger("GetUp");
                getUpTriggered = true;
            }
        }
    }

    public override void ExitState()
    {
        player.isCrashing = false;
        player.stats.currentStatus = PlayerStats.Status.normal;
    }

    public override void CheckStateUpdate()
    {
        // only exit after get-up animation has finished playing
        if (!player.anim.GetBool("InHit"))
        {
            SwitchState(player.IdleState);
        }
    }
}