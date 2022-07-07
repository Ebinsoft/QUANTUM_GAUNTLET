using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableItem : MonoBehaviour
{
    public ItemSpawner spawner;

    public abstract void Cleanup();

    public abstract void OnTriggerEnter(Collider other);
}
