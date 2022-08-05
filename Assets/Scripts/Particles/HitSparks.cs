using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HitSparks : MonoBehaviour
{
    [Tooltip("Particles played on Small, Medium, and Large")]
    public List<ParticleSystem> smallParticles;

    [Tooltip("Particles played on Medium, and Large")]
    public List<ParticleSystem> mediumParticles;

    [Tooltip("Particles only played on Large")]
    public List<ParticleSystem> largeParticles;

    [Tooltip("Minimum damage for small particles to play")]
    public int smallDamageThreshold;

    [Tooltip("Minimum damage for medium particles to play")]
    public int mediumDamageThreshold;

    [Tooltip("Minimum damage for large particles to play")]
    public int largeDamageThreshold;


    public void Play(int damage)
    {
        if (damage >= smallDamageThreshold)
        {
            foreach (var effect in smallParticles)
            {
                effect.Play();
            }
        }

        if (damage >= mediumDamageThreshold)
        {
            foreach (var effect in mediumParticles)
            {
                effect.Play();
            }
        }

        if (damage >= largeDamageThreshold)
        {
            foreach (var effect in largeParticles)
            {
                effect.Play();
            }
        }
    }
}
