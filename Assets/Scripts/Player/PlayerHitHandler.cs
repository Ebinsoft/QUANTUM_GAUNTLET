using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    private PlayerManager player;

    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public bool handleHit(HitData hitData)
    {
        AttackInfo attack = hitData.attack;

        if (player.stats.canTakeDamage)
        {
            player.stats.health -= attack.damage;
        }

        if (player.stats.canGetStunned)
        {
            player.triggerHit = hitData;

            // if (attack.knockback > 0)
            // {
            //     Vector3 knockbackDirection = (transform.position - attackerTransform.position).normalized;
            //     player.rotationTarget.x = -knockbackDirection.x;
            //     player.rotationTarget.y = -knockbackDirection.z;
            //     player.currentMovement = knockbackDirection * attack.knockback;
            // }

            player.animEffects.PlayHitLag(attack.hitlagTime);
        }

        return player.stats.canGiveRecoil;
    }
}
