using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    [Serializable]
    public struct AttackRef
    {
        public string name;
        public Collider[] colliders;
    }

    public AttackRef[] attacks;
    private Dictionary<String, Collider[]> attackDict;

    private List<Rigidbody> hitRigidBodies;

    void Start()
    {
        // load list of attacks into dictionary for better lookup
        attackDict = attacks.ToDictionary(a => a.name, a => a.colliders);

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
            }
        }
    }

    public void ActivateAttackHitboxes(string attackName)
    {
        try
        {
            hitRigidBodies = new List<Rigidbody>();
            Collider[] colliders = attackDict[attackName];
            foreach (Collider collider in colliders)
            {
                collider.enabled = true;
            }
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError("Attack does not exist: " + attackName);
        }
    }

    public void DeactivateAttackHitboxes(string attackName)
    {
        try
        {
            Collider[] colliders = attackDict[attackName];
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

            DealDamage();
        }
        catch (KeyNotFoundException)
        {
            Debug.LogError("Attack does not exist: " + attackName);
        }
    }

    private void DealDamage()
    {
        foreach (Rigidbody rb in hitRigidBodies)
        {
            rb.gameObject.GetComponent<DummyHealth>().currentHealth -= 1;
        }
    }
}
