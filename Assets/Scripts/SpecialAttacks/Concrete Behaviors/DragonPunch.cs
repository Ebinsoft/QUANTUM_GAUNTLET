using System.Collections.Generic;
using UnityEngine;

public class DragonPunch : SpecialAttackBehavior
{
    UnityEngine.Object fireFistPrefab = Resources.Load("Prefabs/Projectiles/FireFist");
    InterruptableSound chargingSound;
    PlayerParticleEffects particleEffects;

    bool buttonWasReleased;
    bool canPunch;

    float chargeTimer;
    float maxChargeTime = 1.0f;

    float manaDrainPerSecond = 200;

    // distance away from player's XZ position to spawn projectile
    float spawnDistance = 0.5f;

    // distance above player's Y position to spawn projectile
    float spawnHeight = 0.4f;

    float oldSpeed;
    float oldRotation;

    public override void OnEnter()
    {
        buttonWasReleased = false;
        canPunch = false;
        chargeTimer = 0f;

        player.currentState.canRotate = true;
        player.currentState.canMove = true;
        oldSpeed = player.playerSpeed;
        oldRotation = player.rotationSpeed;
        player.playerSpeed = 2.0f;
        player.rotationSpeed = 2f;

        Sound s = AudioManager.magicSounds[MagicSound.ChargeUp];
        chargingSound = AudioManager.CreateInterruptable(s, parent: player.transform);
        chargingSound.Play();

        particleEffects = player.GetComponent<PlayerParticleEffects>();
    }

    public override void Update()
    {
        chargeTimer += Time.deltaTime;
        if (!player.isSpecial2Pressed) buttonWasReleased = true;

        // slowly drain mana over time while charging
        if (canPunch)
        {
            player.stats.DrainMana(manaDrainPerSecond * Time.deltaTime);
            if (player.stats.mana <= 0)
            {
                player.anim.SetTrigger("ReleasePunch");
                ActivatePunch(chargeTimer / maxChargeTime);
            }
        }

        if (canPunch && buttonWasReleased)
        {
            player.anim.SetTrigger("ReleasePunch");
            ActivatePunch(chargeTimer / maxChargeTime);
        }

    }

    private void ActivatePunch(float chargePercent)
    {
        canPunch = false;

        chargePercent = Mathf.Min(1, chargePercent);

        Vector3 spawnPoint = player.transform.position + player.transform.forward * spawnDistance;
        spawnPoint.y += spawnHeight;
        Quaternion spawnRot = Quaternion.LookRotation(player.transform.forward, Vector3.up);

        var extraParams = new Dictionary<string, object>();
        extraParams.Add("ChargePercent", chargePercent);

        GameObject fireCone = SpawnProjectile(fireFistPrefab, spawnPoint, spawnRot, extraParams: extraParams);
        chargingSound.StopAndDestroy();
        AudioManager.PlayAt(FireSound.ExplosionBig, player.gameObject);
        particleEffects.StopChargingEffect();
    }

    public override void OnExit()
    {
        // cleanup in case of interruption
        player.currentState.canRotate = false;
        player.currentState.canMove = false;
        player.playerSpeed = oldSpeed;
        player.rotationSpeed = oldRotation;
        chargingSound.StopAndDestroy();
        particleEffects.StopChargingEffect();
    }

    public override void OnHit(Collider other) { }

    public override void TriggerAction(int actionID)
    {
        switch (actionID)
        {
            case 0:
                // punch has reached its minimum charge time
                canPunch = true;
                chargeTimer = 0;
                particleEffects.StartChargingEffectAt(player.transform.position);
                break;

            case 1:
                // punch has reached its maximum charge time, forced release
                ActivatePunch(1f);
                break;

            default:
                Debug.LogError("Invalid actionID: " + actionID);
                break;
        }
    }
}