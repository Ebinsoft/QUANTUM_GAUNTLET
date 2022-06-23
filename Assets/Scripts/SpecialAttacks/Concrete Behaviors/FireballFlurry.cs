using System.Collections.Generic;
using UnityEngine;

public class FireballFlurry : SpecialAttackBehavior
{
    UnityEngine.Object fireballPrefab = Resources.Load("Prefabs/Projectiles/Fireball");

    // angle between the two outer-most projectile's directions
    float spreadAngle = 60;

    // number of projectiles to shoot
    int numProjectiles = 5;

    // distance away from player's XZ position to spawn projectiles
    float spawnDistance = 0.75f;

    // distance above player's Y position to spawn projectiles
    float spawnHeight = 0.75f;

    List<Vector2> projectileDirections;

    public override void OnEnter()
    {
        // Compute the trajectory directions for all fireballs
        float deltaAngle = spreadAngle / (numProjectiles - 1);

        Vector2 front = new Vector2(player.transform.forward.x, player.transform.forward.z);
        Vector2 leftMost = Rotate(front, -spreadAngle / 2);

        projectileDirections = new List<Vector2>();
        for (int i = 0; i < numProjectiles; i++)
        {
            Vector2 dir = Rotate(leftMost, deltaAngle * i);
            projectileDirections.Add(dir);
        }
    }

    public override void Update() { }

    public override void OnExit() { }

    public override void TriggerAction(int actionID)
    {
        if (actionID > projectileDirections.Count - 1)
        {
            Debug.LogError("Invalid actionID: " + actionID);
            return;
        }

        Vector2 dir2 = projectileDirections[actionID];
        Vector3 dir3 = new Vector3(dir2.x, 0, dir2.y);
        Vector3 spawnPoint = player.transform.position + dir3 * spawnDistance;
        spawnPoint.y += spawnHeight;

        SpawnProjectile(fireballPrefab, spawnPoint, Quaternion.LookRotation(dir3, Vector3.up));
    }

    private static Vector2 Rotate(Vector2 v, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        return new Vector2(
            cos * v.x - sin * v.y,
            sin * v.x + cos * v.y
        );
    }
}