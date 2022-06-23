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

    public void TriggerSpecialAttackAction(int actionID)
    {
        if (attackHandler.activeSpecialBehavior != null)
        {
            attackHandler.activeSpecialBehavior.TriggerAction(actionID);
        }
    }
}
