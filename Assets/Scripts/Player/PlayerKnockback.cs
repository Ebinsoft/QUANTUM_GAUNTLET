using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnockback : MonoBehaviour
{
    private PlayerManager player;

    private HitData activeHit;

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


    void Start()
    {
        player = GetComponent<PlayerManager>();
    }

    public void ApplyKnockback(HitData hitData)
    {
        activeHit = hitData;

        // pull out stun time for easier access
        stunTime = hitData.attack.stunTime;

        // compute the directional vector of the attack
        attackDirection = (player.transform.position - hitData.origin.position).normalized;

        // if there is knockback, turn the player towards the attacker
        if (hitData.attack.knockbackMagnitude > 0)
        {
            player.rotationTarget.x = -attackDirection.x;
            player.rotationTarget.y = -attackDirection.z;
        }

        InitializeHorizontalKnockback();
        applyKnockup();
    }

    private void InitializeHorizontalKnockback()
    {
        // begin stun timer
        startTime = Time.time;

        knockbackDist = activeHit.attack.knockback.x;

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

    private void applyKnockup()
    {
        player.gravity = -51;
        float timeToApex = Mathf.Sqrt((-2f / player.gravity) * activeHit.attack.knockback.y);
        float knockupVelocity = (2 * activeHit.attack.knockback.y) / timeToApex;

        player.currentMovement.y = knockupVelocity;
    }

    void Update()
    {
        if (stunTime > 0 && activeHit.attack.knockback.x > 0)
        {
            updateKnockback();
        }
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
}
