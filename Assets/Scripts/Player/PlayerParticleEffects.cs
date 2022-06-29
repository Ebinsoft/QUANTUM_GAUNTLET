using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour
{
    PlayerManager player;

    public ParticleSystem dustBurst;
    public ParticleSystem dustTrail;
    public ParticleSystem smokeTrail;
    public ParticleSystem fireTrail;

    public ParticleSystem hitParticles;

    public ParticleSystem fireBurst;

    public void PlayLandingEffect()
    {
        dustBurst.Play();
    }

    public void PlayHitEffectAt(Vector3 pos)
    {
        hitParticles.transform.position = pos;
        hitParticles.Play();
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
}
