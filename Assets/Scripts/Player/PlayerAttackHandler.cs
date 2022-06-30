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

    // cached set of all hitboxes attached to player
    private List<ColliderWithDefault> playerHitboxes;

    private AttackInfo activeAttack = null;
    public SpecialAttackBehavior activeSpecialBehavior { get; private set; }

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
    }

    void Update()
    {
        if (activeSpecialBehavior != null)
        {
            activeSpecialBehavior.Update();
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

        // Store colliders with a copy of their transform values
        playerHitboxes = hitboxes.Select(c => new ColliderWithDefault()
        {
            collider = c,
            position = c.transform.localPosition,
            rotation = c.transform.localRotation,
            scale = c.transform.localScale
        }).ToList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activeAttack == null) return;

        bool isHurtbox = other.gameObject.layer == LayerMask.NameToLayer("Hurtbox");
        if (!isHurtbox) return;
        bool isOpponent = other.attachedRigidbody.tag != this.tag;
        bool wasAlreadyHit = hitRigidBodies.Contains(other.attachedRigidbody);

        if (isHurtbox && isOpponent && !wasAlreadyHit)
        {
            if (activeSpecialBehavior != null) activeSpecialBehavior.OnHit(other);
            hitRigidBodies.Add(other.attachedRigidbody);
            HitPlayer(other.attachedRigidbody.gameObject, other.transform.position);
        }
    }

    private void HitPlayer(GameObject playerObj, Vector3 hitPoint)
    {
        Vector3 direction = hitPoint - transform.position;
        HitData hitData = new HitData() { attack = activeAttack, direction = direction };

        bool hitResolved = playerObj.GetComponent<PlayerHitHandler>().handleHit(hitData);
        if (hitResolved)
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
        if (attack.hasSpecialBehavior)
        {
            activeSpecialBehavior = (SpecialAttackBehavior)attack.specialBehavior.CreateInstance();
            activeSpecialBehavior.player = GetComponent<PlayerManager>();
            activeSpecialBehavior.OnEnter();
        }

        // reset list of rigidbodies hit by attack
        hitRigidBodies = new HashSet<Rigidbody>();
    }

    public void FinishAttack()
    {
        if (activeAttack == null) return;

        CleanupHitboxes();

        // reset active attack
        activeAttack = null;

        if (activeSpecialBehavior != null)
        {
            activeSpecialBehavior.OnExit();
            activeSpecialBehavior = null;
        }
    }

    private void CleanupHitboxes()
    {
        foreach (var hitbox in playerHitboxes)
        {
            hitbox.collider.enabled = false;
            hitbox.collider.transform.localPosition = hitbox.position;
            hitbox.collider.transform.localRotation = hitbox.rotation;
            hitbox.collider.transform.localScale = hitbox.scale;
        }
    }
}
