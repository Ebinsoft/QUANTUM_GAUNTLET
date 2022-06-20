using System;
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
            player.triggerHit = true;

            Action stunAndKnockback = () =>
            {
                if (attack.stunTime > 0)
                {
                    player.playerStun.ApplyStun(hitData.attack.stunTime);
                }

                if (attack.knockbackMagnitude > 0)
                {
                    player.playerKnockback.ApplyKnockback(hitData, hitData.attack.stunTime);
                }
            };

            player.animEffects.PlayHitLag(attack.hitlagTime,
                onComplete: stunAndKnockback);
        }

        return player.stats.canGiveRecoil;
    }
}
