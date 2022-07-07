using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimAssist : MonoBehaviour
{
    PlayerManager player;

    void Start()
    {
        player = GetComponent<PlayerManager>();
    }

    public void TrackNearestOpponent(float distance, float angle)
    {
        var closestOpponent = VersusSceneManager.instance.playerList
            .Where(p => p.tag != this.tag)
            .Where(p => AngleToMe(p.transform.position) < angle / 2)
            .Select(p => new Tuple<GameObject, float>(p, Vector3.Distance(transform.position, p.transform.position)))
            .Where(tup => tup.Item2 < distance)
            .OrderBy(tup => tup.Item2)
            .Select(tup => tup.Item1)
            .FirstOrDefault();

        if (closestOpponent != null)
        {
            Vector3 diff = closestOpponent.transform.position - transform.position;
            diff.y = 0;
            diff.Normalize();

            player.rotationTarget.x = diff.x;
            player.rotationTarget.y = diff.z;
        }
    }

    private float AngleToMe(Vector3 position)
    {
        Vector3 diff = position - transform.position;
        diff.z = transform.forward.z;

        return Vector3.Angle(transform.forward, diff);
    }
}
