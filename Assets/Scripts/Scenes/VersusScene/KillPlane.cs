using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerManager>();

        // if they're already dead, don't kill them again, respawn them immediately
        if (player != null && player.currentState != player.DeadState)
        {
            player.TriggerDeath(delayBeforeExplode: false);
        }
    }
}
