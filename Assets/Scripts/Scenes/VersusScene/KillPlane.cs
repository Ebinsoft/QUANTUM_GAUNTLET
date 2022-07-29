using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerManager>();
        if (player != null)
        {
            player.TriggerDeath(playAnimation: false);
        }
    }
}
