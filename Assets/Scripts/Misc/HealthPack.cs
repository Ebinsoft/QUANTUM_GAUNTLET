using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HealthPack : InteractableItem
{

    public int healthRestored;

    private GameObject go;
    private PlayerManager pm;
    private PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Restore the character's health
    private void RestoreHealth(PlayerManager pm) {
        stats = pm.stats;

        // Actually update health
        if(stats != null) {
            stats.RestoreHealth(healthRestored);
            Debug.Log("Health Restored: " + healthRestored);
            Cleanup();
        } else {
            Debug.Log("Unable to find reference to PlayerStats from PlayerManager.");
        }
    }

    // Destroy this GameObject. Signal the PickupSpawner.
    public override void Cleanup() {
        Destroy(gameObject);
        spawner.ItemTaken();
        Debug.Log("Destroyed");
    }

    // Handle collisions
    public override void OnTriggerEnter(Collider other) {
        go = other.gameObject;
        pm = go.GetComponent<PlayerManager>();

        if(pm != null) {
            // collision with a player character
            Debug.Log("Collision with player");
            RestoreHealth(pm);
        } else {
            // otherwise
            Debug.Log("Collision with other");
        }
    }
}
