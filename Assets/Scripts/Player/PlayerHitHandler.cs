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

    public bool handleHit(AttackInfo attack)
    {
        if (player.stats.canTakeDamage)
        {
            player.stats.health -= attack.damage;
        }

        if (player.stats.canGetStunned)
        {
            player.triggerHit = true;
            player.animEffects.PlayHitlag(attack.hitlagTime);
        }

        return player.stats.canGiveRecoil;
    }
}
