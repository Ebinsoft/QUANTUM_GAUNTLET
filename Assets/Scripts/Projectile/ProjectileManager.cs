using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ProjectileManager : MonoBehaviour
{
    // behavior and attack information
    public SerializedClass behaviorType;
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

        behavior = (ProjectileBehavior)behaviorType.CreateInstance();
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
    SerializedProperty m_behaviorType;
    SerializedProperty m_attack;
    SerializedProperty m_movementSpeed;
    SerializedProperty m_rotationSpeed;

    public override void OnInspectorGUI()
    {
        ProjectileManager obj = target as ProjectileManager;

        // projectile behavior selector
        obj.behaviorType = SubclassSelector.Dropdown<ProjectileBehavior>("Behavior", obj.behaviorType);

        // attack info
        obj.attack = (AttackInfo)EditorGUILayout.ObjectField("Attack", obj.attack, typeof(AttackInfo), false);

        // speeds
        obj.movementSpeed = EditorGUILayout.FloatField("Movement Speed", obj.movementSpeed);
        obj.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", obj.rotationSpeed);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(obj);
        }
    }
}
