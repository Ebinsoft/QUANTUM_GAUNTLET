using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    public AttackInfo[] attacks;
    private Dictionary<String, AttackInfo> attackDict;

    // cached set of all hitboxes attached to player
    private Dictionary<String, Collider> playerHitboxes;

    private AttackInfo activeAttack = null;

    // tracks the distinct players hit during an active attack window
    private HashSet<Rigidbody> hitRigidBodies;

    // references to external components
    public Animator anim;
    private PlayerParticleEffects effects;

    void Start()
    {
        // load particle effects player
        effects = GetComponent<PlayerParticleEffects>();

        // cache player's atached hitboxes
        LoadPlayerHitboxes();

        // disable all hitboxes by default
        foreach (var hitbox in playerHitboxes.Values)
        {
            hitbox.enabled = false;
        }

        // load list of attacks into dictionary for better lookup
        attackDict = attacks.ToDictionary(
            a => a.name,
            a => UnityEngine.Object.Instantiate(a)
        );

        // populate each attack's hitbox fields
        foreach (AttackInfo atk in attackDict.Values)
        {
            atk.hitboxes = new Collider[atk.hitboxNames.Length];
            for (int i = 0; i < atk.hitboxes.Length; i++)
            {
                try
                {
                    atk.hitboxes[i] = playerHitboxes[atk.hitboxNames[i]];
                }
                catch (KeyNotFoundException)
                {
                    Debug.LogErrorFormat(
                        "Hitbox named %s does not exist (specified in %s attack info).",
                        atk.hitboxNames[i],
                        atk.attackName);
                }
            }
        }
    }

    // populate playerHitboxes with all uniquely-named colliders on the hitbox layer
    private void LoadPlayerHitboxes()
    {
        // find all colliders on hitbox layer
        var hitboxes = GetComponentsInChildren<Collider>()
            .Where(c => c.gameObject.layer == LayerMask.NameToLayer("Hitbox"));

        // detect hitboxes with the same name (all hitbox names should be unique)
        hitboxes.Select(c => c.gameObject.name)
            .GroupBy(c => c)
            .Where(g => g.Count() > 1)
            .Select(g => g.First()).ToList()
            .ForEach(c => Debug.LogError("Duplicate hitbox name found: " + c));

        // map hitbox names to collider components
        playerHitboxes = hitboxes.ToDictionary(c => c.gameObject.name, c => c);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hurtbox"))
        {
            if (!hitRigidBodies.Contains(other.attachedRigidbody))
            {
                hitRigidBodies.Add(other.attachedRigidbody);
                HitPlayer(other.attachedRigidbody.gameObject, other.transform.position);
            }
        }
        else
        {
            Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
        }
    }

    private void HitPlayer(GameObject playerObj, Vector3 hitPoint)
    {
        if (activeAttack == null) return;

        // do hitlag for attacking player
        StartCoroutine(HitLag(activeAttack.hitlagTime));

        // play hit particle effect at contact point
        effects.PlayHitEffectAt(hitPoint);

        // make opponent play TakeHit animation (temporary until we have an actual state for this)
        playerObj.GetComponentInChildren<Animator>().SetTrigger("TakeHit");
        // also make them hitlag
        StartCoroutine(playerObj.GetComponentInChildren<PlayerAttackHandler>().HitLag(activeAttack.hitlagTime));

        // deal damage
        playerObj.GetComponent<DummyHealth>().currentHealth -= activeAttack.damage;
    }

    IEnumerator HitLag(float duration)
    {
        // pause animator
        anim.speed = 0;
        yield return new WaitForSeconds(duration);

        // play animator at 2x speed for same duration to catch up
        anim.speed = 2;
        yield return new WaitForSeconds(duration);

        // return to normal speed
        anim.speed = 1;
    }

    public void InitiateAttack(string attackName)
    {
        try
        {
            // search for attack in dict and set as active attack
            AttackInfo attack = attackDict[attackName];
            activeAttack = attack;

            // reset list of rigidbodies hit by attack
            hitRigidBodies = new HashSet<Rigidbody>();

            // turn on hitbox colliders
            foreach (Collider hitbox in attack.hitboxes)
            {
                hitbox.enabled = true;
            }
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError("Attack does not exist: " + attackName);
        }
    }

    public void FinishAttack()
    {
        if (activeAttack == null) return;

        // turn off hitbox colliders
        foreach (Collider hitbox in activeAttack.hitboxes)
        {
            hitbox.enabled = false;
        }

        // reset active attack
        activeAttack = null;
    }
}
