using System.Collections.Generic;
using UnityEngine;

public class DragonPunch : SpecialAttackBehavior
{
    UnityEngine.Object fireFistPrefab = Resources.Load("Prefabs/Projectiles/FireFist");

    bool buttonWasReleased;
    bool canPunch;

    float chargeTimer;
    float maxChargeTime = 1.0f;

    float manaDrainPerSecond = 200;

    // distance away from player's XZ position to spawn projectile
    float spawnDistance = 0.5f;

    // distance above player's Y position to spawn projectile
    float spawnHeight = 0.4f;

    public override void OnEnter()
    {
        buttonWasReleased = false;
        canPunch = false;
        chargeTimer = 0f;
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
    }

    public override void OnExit() { }

    public override void OnHit(Collider other) { }

    public override void TriggerAction(int actionID)
    {
        switch (actionID)
        {
            case 0:
                // punch has reached its minimum charge time
                canPunch = true;
                chargeTimer = 0;
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