using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitHandler : MonoBehaviour
{
    public PlayerManager player;
    // Start is called before the first frame update

    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public bool handleHit(AttackInfo attack)
    {
        Debug.Log("GET HIT");
        Debug.Log(player.stats.currentStatus);
        Debug.Log(player.stats.canGetStunned);
        if (player.stats.canTakeDamage)
        {
            player.stats.health -= attack.damage;
        }

        if (player.stats.canGetStunned)
        {
            player.triggerHit = true;
            StartCoroutine(player.animEffects.HitLag(attack.hitlagTime));
        }

        return player.stats.canGiveRecoil;
    }
}
