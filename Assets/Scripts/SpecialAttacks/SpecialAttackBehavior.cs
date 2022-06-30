using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialAttackBehavior
{
    public PlayerManager player;

    // called once after special attack activates
    public abstract void OnEnter();

    // called every frame
    public abstract void Update();

    // called once after special attack finishes
    public abstract void OnExit();

    // called by PlayerAttackHandler when a player is hit during the special attack
    public abstract void OnHit(Collider other);

    // exposed so that animations can trigger frame-specific actions
    public abstract void TriggerAction(int actionID);

    public GameObject SpawnProjectile(UnityEngine.Object prefab, Vector3 spawnPoint, Quaternion rotation, Dictionary<String, object> extraParams = null)
    {
        GameObject obj = GameObject.Instantiate(prefab, spawnPoint, rotation) as GameObject;
        obj.tag = player.tag;
        obj.GetComponent<ProjectileManager>().extraParams = extraParams;
        return obj;
    }
}
