using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerBaseState
{
    private PlayerManager player;
    private HitData hitAttack;

    // computed as the direction from attacker to me
    private Vector3 attackDirection;

    // how far into the stuntime we begin decelerating
    private float percentAtFullSpeed = 0.5f;

    private float startTime;

    // KNOCKBACK VARIABLES
    private bool knockbackTriggered;
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

        // force the animator to ignore its current transition rules and play the stun animation
        player.anim.Play("Take Hit");

        // pull out hitData and reset the trigger
        hitAttack = player.triggerHit.Value;
        player.triggerHit = null;

        // pull out stun time for easier access
        stunTime = hitAttack.attack.stunTime;

        knockbackTriggered = false;

        // compute the directional vector of the attack
        attackDirection = (player.transform.position - hitAttack.origin.position).normalized;

        // if there is knockback, turn the player towards the attacker
        if (hitAttack.attack.knockback > 0)
        {
            player.rotationTarget.x = -attackDirection.x;
            player.rotationTarget.y = -attackDirection.z;
        }
    }

    private void CalculateKnockbackVariables()
    {
        // begin stun timer
        startTime = Time.time;

        knockbackDist = hitAttack.attack.knockback;

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
    }

    public override void UpdateState()
    {
        if (!player.isHitLagging)
        {
            // this whole block should only run once after hitlag stops
            if (stunTime > 0 && !knockbackTriggered)
            {
                if (hitAttack.attack.knockback > 0)
                {
                    CalculateKnockbackVariables();
                }

                if (hitAttack.attack.knockup > 0)
                {
                    applyKnockup();
                }

                knockbackTriggered = true;
            }

            if (stunTime > 0 && hitAttack.attack.knockback > 0)
            {
                updateKnockback();
            }
        }
    }

    private void applyKnockup()
    {
        float timeToApex = hitAttack.attack.knockupTime / 2;

        player.gravity = (-2 * hitAttack.attack.knockup) / Mathf.Pow(timeToApex, 2);
        float knockupVelocity = (2 * hitAttack.attack.knockup) / timeToApex;
        player.currentMovement.y = knockupVelocity;
    }

    private void updateKnockback()
    {
        if (Time.time < startTime + fullSpeedTime)
        {
            player.currentMovement.x = attackDirection.x * fullSpeed;
            player.currentMovement.z = attackDirection.z * fullSpeed;
        }
        else
        {
            float timeDecelerating = Time.time - (startTime + fullSpeedTime);
            float currentSpeed = Mathf.Clamp((1 - timeDecelerating / decelTime), 0, 1) * fullSpeed;

            float verletSpeed = (currentSpeed + prevFrameSpeed) / 2;

            player.currentMovement.x = verletSpeed * attackDirection.x;
            player.currentMovement.z = verletSpeed * attackDirection.z;

            prevFrameSpeed = currentSpeed;
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
            SwitchState(player.FallingState);
        }
    }
}
