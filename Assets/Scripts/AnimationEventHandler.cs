using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
    public PlayerAttackHandler attackHandler;

    public void ActivateAttackHitboxes(string attackName)
    {
        attackHandler.ActivateAttackHitboxes(attackName);
    }

    public void DeactivateAttackHitboxes(string attackName)
    {
        attackHandler.DeactivateAttackHitboxes(attackName);
    }
}
