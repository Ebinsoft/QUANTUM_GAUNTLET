using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour
{
    PlayerManager player;

    public ParticleSystem landParticles;

    public ParticleSystem hitParticles;

    public ParticleSystem fireParticles;

    public void PlayLandingEffect()
    {
        landParticles.Play();
    }

    public void PlayHitEffectAt(Vector3 pos)
    {
        hitParticles.transform.position = pos;
        hitParticles.Play();
    }

    public void PlayFireBurstAt(Vector3 pos)
    {
        fireParticles.transform.position = pos;
        fireParticles.Play();
    }
}
