using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DeathExplosion : MonoBehaviour
{
    CinemachineImpulseSource impulseSource;
    ParticleSystem particles;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        particles = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        particles.Play();
        impulseSource.GenerateImpulse();
        AudioManager.PlayAt(FireSound.DeathExplosion, transform.root.gameObject);
    }
}
