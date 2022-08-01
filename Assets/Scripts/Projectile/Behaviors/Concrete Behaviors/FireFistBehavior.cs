using System;
using System.Collections;
using UnityEngine;

public class FireFistBehavior : ProjectileBehavior
{
    private float maxLifetime = 20f / 60;
    private float lifetime;

    private float chargePercent;
    private float baseRange = 2.5f;

    private float damageScaling = 4f;
    private float knockbackScaling = 2.5f;
    private float sizeScaling = 2f;
    private float rangeScaling = 4f;

    HitSparks hitSparks;

    public override void OnSpawn()
    {
        chargePercent = (float)projectile.extraParams["ChargePercent"];

        float damageMultiplier = Mathf.Lerp(1, damageScaling, chargePercent);
        projectile.attack.damage = (int)(projectile.attack.damage * damageMultiplier);

        float knockbackMultiplier = Mathf.Lerp(1, knockbackScaling, chargePercent);
        projectile.attack.knockbackMagnitude *= knockbackMultiplier;

        float scaleMultiplier = Mathf.Lerp(1, sizeScaling, chargePercent);
        projectile.gameObject.transform.localScale *= scaleMultiplier;

        float rangeMultiplier = Mathf.Lerp(1, rangeScaling, chargePercent);
        float range = baseRange * rangeMultiplier;
        float speed = range / maxLifetime;
        projectile.movementSpeed = speed;

        hitSparks = projectile.transform.Find("Hit Sparks").GetComponent<HitSparks>();

        lifetime = 0;
    }

    public override void OnCollision()
    {
        // play hitlag
        float hitlagDuration = 0.1f;
        projectile.FreezeMovement(hitlagDuration);
        lifetime -= hitlagDuration;

        // play impact sound
        AudioManager.PlayAt(FireSound.ExplosionMedium, projectile.gameObject);

        // play hitsparks
        hitSparks.Play(projectile.attack.damage);
    }

    public override void OnDestroy()
    {
        Animator anim = projectile.GetComponent<Animator>();
        anim.Play("DieOut");
    }

    public override void Update()
    {
        lifetime += Time.deltaTime;

        if (lifetime >= maxLifetime)
        {
            projectile.SelfDestruct(0.25f);
        }
    }
}