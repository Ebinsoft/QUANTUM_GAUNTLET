using UnityEngine;

public class FireFistBehavior : ProjectileBehavior
{
    private float timeSpentMoving = 15f / 60;
    private float maxLifetime = 30f / 60;
    private float lifetime;

    private float chargePercent;
    private float baseRange = 1.2f;

    private float damageScaling = 1.5f;
    private float knockbackScaling = 2f;
    private float sizeScaling = 2f;
    private float rangeScaling = 2f;

    public override void OnSpawn()
    {
        chargePercent = (float)projectile.extraParams["ChargePercent"];

        float scaleMultiplier = Mathf.Lerp(1, sizeScaling, chargePercent);
        projectile.gameObject.transform.localScale *= scaleMultiplier;

        float rangeMultiplier = Mathf.Lerp(1, rangeScaling, chargePercent);
        float range = baseRange * rangeMultiplier;
        float speed = range / timeSpentMoving;
        projectile.movementSpeed = speed;

        lifetime = 0;
    }

    public override void OnCollision()
    {
    }

    public override void OnDestroy()
    {
    }

    public override void Update()
    {
        lifetime += Time.deltaTime;

        if (lifetime >= timeSpentMoving)
        {
            projectile.movementSpeed = 0;
        }

        if (lifetime >= maxLifetime)
        {
            projectile.SelfDestruct(0.1f);
        }
    }
}