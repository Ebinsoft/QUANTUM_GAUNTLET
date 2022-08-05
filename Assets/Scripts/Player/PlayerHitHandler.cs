using System;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour, IHitHandler
{
    private PlayerManager player;

    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public bool handleHit(HitData hitData)
    {
        AttackInfo attack = hitData.attack;

        // don't resolve hit if we are already dead
        if (player.stats.health <= 0) return false;

        if (player.stats.canTakeDamage)
        {
            player.stats.health -= attack.damage;
        }

        if (player.stats.canGetStunned)
        {
            player.triggerHit = true;

            // if player got interrupted out of an attack, make sure to clean up
            player.GetComponent<PlayerAttackHandler>().FinishAttack();
            player.ResetAllAnimatorTriggers();

            if (attack.stunTime > 0)
            {
                player.playerStun.ApplyStun(attack.stunTime + attack.hitlagTime);
            }

            Action applyPlayerKnockback = () =>
            {
                if (attack.knockbackMagnitude > 0)
                {
                    player.playerKnockback.ApplyKnockback(hitData, hitData.attack.stunTime);
                }
            };

            player.animEffects.PlayHitLag(attack.hitlagTime,
                onComplete: applyPlayerKnockback);
        }

        return player.stats.canGiveRecoil;
    }
}
