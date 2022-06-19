using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private PlayerManager player;
    private HitData hitAttack;

    // computed as the direction from attacker to me
    private Vector3 knockbackDirection;

    // how far into the stuntime we begin decelerating
    private float percentAtFullSpeed = 0.5f;

    private float startTime;

    // KNOCKBACK VARIABLES
    private float stunTime, knockbackDist;
    private float fullSpeedTime, decelTime;
    private float fullSpeedDist, decelDist;
    private float fullSpeed;

    // tracks previous frame's speed for verlet calculation
    private float prevFrameSpeed;

    public PlayerHitState(PlayerManager psm) : base(psm)
    {
        player = psm;
    }

    public override void EnterState()
    {
        player.isHit = true;

        // pull out hitData and reset the trigger
        hitAttack = player.triggerHit.Value;
        player.triggerHit = null;

        if (hitAttack.attack.stunTime > 0)
        {
            initiateStun();
        }

        // force the animator to ignore its current transition rules and play the stun animation
        player.anim.Play("Take Hit");

        // begin stun timer
        startTime = Time.time;
    }

    private void initiateStun()
    {
        knockbackDist = hitAttack.attack.knockback;
        stunTime = hitAttack.attack.stunTime;

        // calculate time ratios
        fullSpeedTime = percentAtFullSpeed * stunTime;
        decelTime = (1 - percentAtFullSpeed) * stunTime;

        // calculate distance ratios
        decelDist = (decelTime / (2 * fullSpeedTime + decelTime)) * knockbackDist;
        fullSpeedDist = knockbackDist - decelDist;

        // calculate initial knockback speed
        fullSpeed = fullSpeedDist / fullSpeedTime;

        // reset verlet previous frame speed to full speed
        prevFrameSpeed = fullSpeed;

        // calculate knockback direction
        knockbackDirection = (player.transform.position - hitAttack.origin.position).normalized;

        // point player towards the attacker
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
                // float currentDecTime = Time.time - (startTime + fullSpeedTime);
                // float timeDecelerating = (currentDecTime + prevDecTime) / 2f;
                // player.currentMovement.x = (1 - timeDecelerating / decelTime) * fullSpeed * knockbackDirection.x;
                // player.currentMovement.z = (1 - timeDecelerating / decelTime) * fullSpeed * knockbackDirection.z;
                // prevDecTime = currentDecTime;

                float timeDecelerating = Time.time - (startTime + fullSpeedTime);
                float currentSpeed = Mathf.Clamp((1 - timeDecelerating / decelTime),0,1) * fullSpeed;

                float verletSpeed = (currentSpeed + prevFrameSpeed) / 2;

                player.currentMovement.x = verletSpeed * knockbackDirection.x;
                player.currentMovement.z = verletSpeed * knockbackDirection.z;

                prevFrameSpeed = currentSpeed;
            }
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
