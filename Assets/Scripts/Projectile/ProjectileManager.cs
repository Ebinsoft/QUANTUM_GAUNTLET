using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectileManager : MonoBehaviour
{
    // behavior and attack information
    public string behaviorName;
    public ProjectileBehavior behavior;
    public AttackInfo attack;

    // movement variables
    public float movementSpeed;
    public float rotationSpeed;
    public Vector3 rotationTargetDirection;

    // rigidbodies
    private Rigidbody projectileRigidbody;
    private HashSet<Rigidbody> hitRigidbodies;

    void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        hitRigidbodies = new HashSet<Rigidbody>();

        rotationTargetDirection = transform.forward;

        Type behaviorType = Type.GetType(behaviorName);
        behavior = (ProjectileBehavior)Activator.CreateInstance(behaviorType);
        behavior.projectile = this;
        behavior.OnSpawn();
    }

    void Update()
    {
        behavior.Update();
    }

    void FixedUpdate()
    {
        UpdateRotation();
        UpdateMovement();
    }

    void UpdateRotation()
    {
        if ((transform.forward - rotationTargetDirection.normalized).magnitude > 0.001f)
        {
            Quaternion currentRotation = transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(rotationTargetDirection);
            Quaternion newRotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
            projectileRigidbody.MoveRotation(newRotation);
        }
    }

    void UpdateMovement()
    {
        // always move a projectile in its forward direction
        Vector3 deltaMovement = transform.forward * movementSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + deltaMovement;
        projectileRigidbody.MovePosition(newPosition);
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO: make sure player hit is on a different team

        if (other.gameObject.layer == LayerMask.NameToLayer("Hurtbox")
            && !hitRigidbodies.Contains(other.attachedRigidbody))
        {
            GameObject playerHit = other.attachedRigidbody.gameObject;
            HitData hitData = new HitData() { attack = attack, origin = transform };
            bool hitResolved = playerHit.GetComponent<PlayerHitHandler>().handleHit(hitData);

            if (hitResolved)
            {
                behavior.OnCollision();
                hitRigidbodies.Add(other.attachedRigidbody);
            }
        }
    }

    // should be called by the projectile behavior when it is ready to die
    public void SelfDestruct(float delay)
    {
        behavior.OnDestroy();
        Destroy(gameObject, delay);
    }
}


[CustomEditor(typeof(ProjectileManager))]
public class ProjectileManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProjectileManager obj = target as ProjectileManager;

        // projectile behavior selector
        var behaviors = ProjectileBehavior.GetAllProjectileBehaviors();
        string[] behaviorNames = behaviors.Select(b => b.ToString()).ToArray();
        int behaviorIdx = Math.Max(0, Array.IndexOf(behaviorNames, obj.behaviorName));

        behaviorIdx = EditorGUILayout.Popup("Behavior", behaviorIdx, behaviorNames);
        obj.behaviorName = behaviorNames[behaviorIdx];

        // attack info
        obj.attack = (AttackInfo)EditorGUILayout.ObjectField("Attack", obj.attack, typeof(AttackInfo), false);

        // speeds
        obj.movementSpeed = EditorGUILayout.FloatField("Movement Speed", obj.movementSpeed);
        obj.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", obj.rotationSpeed);

        EditorUtility.SetDirty(obj);
    }
}
