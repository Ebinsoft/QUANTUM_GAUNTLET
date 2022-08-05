using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour
{
    PlayerManager player;

    public HitSparks hitSparks;
    public ParticleSystem dustBurst;
    public ParticleSystem dustTrail;
    public ParticleSystem smokeTrail;
    public ParticleSystem fireTrail;
    public ParticleSystem fireBurst;
    public ParticleSystem chargingParticles;
    public DeathExplosion deathExplosion;

    public void PlayLandingEffect()
    {
        dustBurst.Play();
    }

    public void PlayHitSparksAt(Vector3 pos, int damage)
    {
        hitSparks.transform.position = pos;
        hitSparks.Play(damage);
    }

    public void PlayFireBurstAt(Vector3 pos)
    {
        fireBurst.transform.position = pos;
        fireBurst.Play();
    }

    public void StartDashingEffect()
    {
        dustTrail.Play();
    }

    public void StopDashingEffect()
    {
        dustTrail.Stop();
    }

    public void StartFireDashingEffect()
    {
        fireTrail.Play();
        smokeTrail.Play();
    }

    public void StopFireDashingEffect()
    {
        fireTrail.Stop();
        smokeTrail.Stop();
    }

    public void StartChargingEffectAt(Vector3 pos)
    {
        chargingParticles.transform.position = pos;
        chargingParticles.Play();
    }

    public void StopChargingEffect()
    {
        chargingParticles.Stop();
    }

    public void PlayDeathExplosion()
    {
        deathExplosion.Play();
    }
}
