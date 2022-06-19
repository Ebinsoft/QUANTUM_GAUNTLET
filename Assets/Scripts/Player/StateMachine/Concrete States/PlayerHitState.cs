using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private PlayerManager player;
    private HitData hitAttack;

    private Vector3 knockbackDirection;

    private float percentAtFullSpeed = 0.5f;

    private float startTime;

    private float stunTime, knockbackDist;
    private float fullSpeedTime, decelTime;
    private float fullSpeedDist, decelDist;
    private float fullSpeed;
    private float decelRate;
    private float prevDecTime;

    public PlayerHitState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }

    public override void EnterState()
    {
        player.isHit = true;
        hitAttack = player.triggerHit.Value;
        player.triggerHit = null;

        if (hitAttack.attack.stunTime > 0)
        {
            initiateStun();
        }

        // force the animator to ignore its current transition rules and play the stun animation
        player.anim.Play("Take Hit");

        startTime = Time.time;
        prevDecTime = startTime + fullSpeedTime;
    }

    private void initiateStun()
    {
        knockbackDist = hitAttack.attack.knockback;
        stunTime = hitAttack.attack.stunTime;

        // calculate time ratios
        fullSpeedTime = percentAtFullSpeed * stunTime;
        decelTime = (1 - percentAtFullSpeed) * stunTime;
        // calculate distance ratios
        decelDist = ((1 - percentAtFullSpeed) / 2) * knockbackDist;
        fullSpeedDist = knockbackDist - decelDist;

        fullSpeed = fullSpeedDist / fullSpeedTime;

        // calculate knockback direction
        knockbackDirection = (player.transform.position - hitAttack.origin.position).normalized;

        player.rotationTarget.x = -knockbackDirection.x;
        player.rotationTarget.y = -knockbackDirection.z;
    }

    public override void UpdateState()
    {
        if (hitAttack.attack.stunTime > 0)
        {
            if (Time.time < startTime + fullSpeedTime)
            {
                player.currentMovement.x = knockbackDirection.x * fullSpeed;
                player.currentMovement.z = knockbackDirection.z * fullSpeed;
            }
            else
            {
                // velocity verlet integration
                float currentDecTime = Time.time - (startTime + fullSpeedTime);
                float timeDecelerating = (currentDecTime + prevDecTime) / 2f;
                player.currentMovement.x = (1 - timeDecelerating / decelTime) * fullSpeed * knockbackDirection.x;
                player.currentMovement.z = (1 - timeDecelerating / decelTime) * fullSpeed * knockbackDirection.z;
                prevDecTime = currentDecTime;
            }
            Debug.Log(player.currentMovement.magnitude);
        }

    }

    public override void ExitState()
    {
        player.isHit = false;
    }

    public override void CheckStateUpdate()
    {
        if (Time.time - startTime < stunTime) return;


        if (!player.anim.GetBool("InHit") && player.characterController.isGrounded)
        {
            SwitchState(player.IdleState);
        }

        else if (!player.anim.GetBool("InHit") && !player.characterController.isGrounded)
        {
            SwitchState(player.JumpingState);
        }
    }
}
