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
    }

    void Start()
    {
        player = GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (player.isStunned && Time.time >= startTime + stunDuration)
        {
            player.isStunned = false;
        }
    }
}
