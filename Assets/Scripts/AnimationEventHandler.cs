using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public PlayerAttackHandler attackHandler;
    public PlayerParticleEffects effects;

    public void InitiateAttack(string attackName)
    {
        attackHandler.InitiateAttack(attackName);
    }

    public void FinishAttack()
    {
        attackHandler.FinishAttack();
    }

    public void PlayLandingEffect()
    {
        effects.PlayLandingEffect();
    }
}
