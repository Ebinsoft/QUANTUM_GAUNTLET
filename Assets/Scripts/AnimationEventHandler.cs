using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public PlayerAttackHandler attackHandler;
    public PlayerParticleEffects effects;

    public void PlayLandingEffect()
    {
        effects.PlayLandingEffect();
    }
}
