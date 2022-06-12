using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    [Serializable]
    public struct AttackRef
    {
        public string name;
        public int damage;
        public float stunTime;
        public float hitlagTime;
        public Collider[] colliders;
    }

    public Jab jab;
    public AttackRef[] attacks;
    private Dictionary<String, AttackRef> attackDict;
    private AttackRef? activeAttack;

    private List<Rigidbody> hitRigidBodies;

    public Animator anim;
    private PlayerParticleEffects effects;

    void Start()
    {
        // Jab?
        jab = new Jab(gameObject);
        // load particle effects player
        effects = GetComponent<PlayerParticleEffects>();

        // load list of attacks into dictionary for better lookup
        attackDict = attacks.ToDictionary(a => a.name, a => a);

        // disable all hitboxes by default
        foreach (var attack in attacks)
        {
            foreach (var collider in attack.colliders)
            {
                collider.enabled = false;
            }
        }
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
        StartCoroutine(HitLag(activeAttack.Value.hitlagTime));

        // play hit particle effect at contact point
        effects.PlayHitEffectAt(hitPoint);

        // make opponent play TakeHit animation (temporary until we have an actual state for this)
        playerObj.GetComponentInChildren<Animator>().SetTrigger("TakeHit");
        // also make them hitlag
        StartCoroutine(playerObj.GetComponentInChildren<PlayerAttackHandler>().HitLag(activeAttack.Value.hitlagTime));

        // deal damage
        playerObj.GetComponent<DummyHealth>().currentHealth -= activeAttack.Value.damage;
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
            // search for attackref in dict and set as active attack
            AttackRef attack = attackDict[attackName];
            activeAttack = attack;

            // reset list of rigidbodies hit by attack
            hitRigidBodies = new List<Rigidbody>();

            // turn on hitbox colliders
            foreach (Collider collider in attack.colliders)
            {
                collider.enabled = true;
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
        foreach (Collider collider in activeAttack.Value.colliders)
        {
            collider.enabled = false;
        }

        // reset active attack
        activeAttack = null;
    }

    private void DealDamage(int amount)
    {
        foreach (Rigidbody rb in hitRigidBodies)
        {
            rb.gameObject.GetComponent<DummyHealth>().currentHealth -= 1;
        }
    }
}
