using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{
    private PlayerManager player;

    private float startTime, stunDuration;

    public void ApplyStun(float duration)
    {
        startTime = Time.time;
        stunDuration = duration;
        player.isStunned = true;
        player.anim.SetBool("IsStunned", true);
    }

    private void CancelStun(GameObject obj)
    {
        player.isStunned = false;
        player.anim.SetBool("IsStunned", false);
    }

    void Start()
    {
        player = GetComponent<PlayerManager>();

        // cancel any lingering stun on player death
        player.stats.onPlayerDie += CancelStun;
    }

    void Update()
    {
        if (player.isStunned && Time.time >= startTime + stunDuration)
        {
            player.isStunned = false;
            player.anim.SetBool("IsStunned", false);
        }
    }
}
