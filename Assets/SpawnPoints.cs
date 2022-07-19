using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnPoints : MonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();
    private static System.Random rng = new System.Random();

    // When you just want a random spawn point (like respawning)
    public Vector3 GetSpawnPoint()
    {
        int r = Random.Range(0, spawnPoints.Count);
        return  transform.position + spawnPoints[r].position;
    }

    // For when you don't want people to spawn on top of each other(like game start)
    public List<Vector3> GetMututallyExclusiveSpawnPoints(int numPoints)
    {
        Shuffle<Transform>(spawnPoints);
        List<Transform> shuffledPoints = spawnPoints.Take(numPoints).ToList();

        return shuffledPoints.Select(c => transform.position + c.position).ToList();
    }

    // Fisher-Yates shuffle which seems sufficiently random
    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
