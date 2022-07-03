using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public PlayerAttackHandler attackHandler;
    public PlayerParticleEffects effects;

    private AudioManager audioManager = null;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public void PlayLandingEffect()
    {
        effects.PlayLandingEffect();
    }

    public void PlayMiscAttackSFX(MiscAttackSound sound)
    {
        audioManager.PlayAt(sound, transform.position);
    }

    public void PlayMovementSFX(MovementSound sound)
    {
        audioManager.PlayAt(sound, transform.position);
    }

    public void TriggerSpecialAttackAction(int actionID)
    {
        if (attackHandler.activeSpecialBehavior != null)
        {
            attackHandler.activeSpecialBehavior.TriggerAction(actionID);
        }
    }
}
