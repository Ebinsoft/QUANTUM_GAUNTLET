using UnityEngine;

public class FireballBehavior : ProjectileBehavior
{
    private float maxLifetime = 5;
    private float lifetime;

    private ParticleSystem explosion;
    private ParticleSystem fireTrail;
    private GameObject model;

    public override void OnSpawn()
    {
        lifetime = 0;

        explosion = projectile.transform.Find("Explosion").GetComponent<ParticleSystem>();
        fireTrail = projectile.transform.Find("Fire Trail").GetComponent<ParticleSystem>();
        model = projectile.transform.Find("Model").gameObject;
    }

    public override void OnCollision()
    {
        projectile.movementSpeed = 0;
        explosion.Play();
        projectile.SelfDestruct(0.6f);
    }

    public override void OnDestroy()
    {
        model.SetActive(false);
        fireTrail.Stop();
    }

    public override void Update()
    {
        lifetime += Time.deltaTime;

        if (lifetime >= maxLifetime)
        {
            projectile.SelfDestruct(0.5f);
        }
    }
}