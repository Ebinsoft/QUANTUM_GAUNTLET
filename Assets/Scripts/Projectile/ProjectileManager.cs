using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    // behavior and attack information
    public SerializedClass behaviorType;
    public ProjectileBehavior behavior;
    public AttackInfo attack;

    // custom parameters for projectile behaviors, should always be set before Start is called
    public Dictionary<String, object> extraParams;

    // movement variables
    public float movementSpeed;
    public float rotationSpeed;
    public Vector3 rotationTargetDirection;
    public Vector3 originPosition;

    // rigidbodies
    private Rigidbody projectileRigidbody;
    private HashSet<Rigidbody> hitRigidbodies;

    // Hitlag
    private IEnumerator FreezeRoutine;

    void Awake()
    {
        projectileRigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        hitRigidbodies = new HashSet<Rigidbody>();

        rotationTargetDirection = transform.forward;
        originPosition = transform.position;

        // make a copy of the AttackInfo so that we can modify it if we want
        attack = Instantiate(attack);

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
        bool isHurtbox = other.gameObject.layer == LayerMask.NameToLayer("Hurtbox");
        bool isOpponent = other.attachedRigidbody.tag != this.tag;
        bool wasAlreadyHit = hitRigidbodies.Contains(other.attachedRigidbody);

        if (isHurtbox && isOpponent && !wasAlreadyHit)
        {
            Vector3 direction = other.transform.position - originPosition;
            HitData hitData = new HitData() { attack = attack, direction = direction };

            GameObject playerHit = other.attachedRigidbody.gameObject;
            bool hitResolved = playerHit.GetComponent<IHitHandler>().handleHit(hitData);

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

    public void FreezeMovement(float duration)
    {
        if (FreezeRoutine != null) StopCoroutine(FreezeRoutine);

        FreezeRoutine = FreezeCoroutine(duration);
        StartCoroutine(FreezeRoutine);
    }

    IEnumerator FreezeCoroutine(float duration)
    {
        var oldSpeed = movementSpeed;
        movementSpeed = 0;

        yield return new WaitForSeconds(duration);

        movementSpeed = oldSpeed;
    }
}