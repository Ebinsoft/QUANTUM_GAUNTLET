using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    private struct ColliderWithDefault
    {
        public Collider collider;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    public AttackInfo[] attacks;
    private Dictionary<String, AttackInfo> attackDict;

    // cached set of all hitboxes attached to player
    private List<ColliderWithDefault> playerHitboxes;

    private AttackInfo activeAttack = null;

    // tracks the distinct players hit during an active attack window
    private HashSet<Rigidbody> hitRigidBodies;

    // references to external components
    public AnimatorEffects animEffects;
    private PlayerParticleEffects effects;

    void Start()
    {
        // load particle effects player
        effects = GetComponent<PlayerParticleEffects>();

        // cache player's atached hitboxes
        LoadPlayerHitboxes();

        // disable all hitboxes by default
        foreach (var hitbox in playerHitboxes)
        {
            hitbox.collider.enabled = false;
        }

        // load list of attacks into dictionary for better lookup
        attackDict = attacks.ToDictionary(
            a => a.name,
            a => UnityEngine.Object.Instantiate(a)
        );
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

        // Store colliders with a copy of their transform values
        playerHitboxes = hitboxes.Select(c => new ColliderWithDefault()
        {
            collider = c,
            position = c.transform.position,
            rotation = c.transform.rotation,
            scale = c.transform.localScale
        }).ToList();
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
            // Debug.Log(LayerMask.LayerToName(other.gameObject.layer));
        }
    }

    private void HitPlayer(GameObject playerObj, Vector3 hitPoint)
    {
        if (activeAttack == null) return;

        if (playerObj.GetComponent<PlayerHitHandler>().handleHit(activeAttack))
        {
            // do hitlag for attacking player
            animEffects.PlayRecoilLag(activeAttack.hitlagTime);

            // play hit particle effect at contact point
            effects.PlayHitEffectAt(hitPoint);
        }
    }

    public void InitiateAttack(AttackInfo attack)
    {
        activeAttack = attack;

        // reset list of rigidbodies hit by attack
        hitRigidBodies = new HashSet<Rigidbody>();
    }

    public void FinishAttack()
    {
        if (activeAttack == null) return;

        CleanupHitboxes();

        // reset active attack
        activeAttack = null;
    }

    private void CleanupHitboxes()
    {
        foreach (var hitbox in playerHitboxes)
        {
            hitbox.collider.enabled = false;
            hitbox.collider.transform.position = hitbox.position;
            hitbox.collider.transform.rotation = hitbox.rotation;
            hitbox.collider.transform.localScale = hitbox.scale;
        }
    }
}
