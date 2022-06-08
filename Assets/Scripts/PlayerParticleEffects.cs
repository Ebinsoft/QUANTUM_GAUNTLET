using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleEffects : MonoBehaviour
{
    PlayerManager player;

    public ParticleSystem landParticles;

    public void PlayLandingEffect()
    {
        landParticles.Play();
    }
}
