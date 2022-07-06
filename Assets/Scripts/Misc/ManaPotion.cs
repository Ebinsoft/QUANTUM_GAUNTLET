using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ManaPotion : MonoBehaviour
{

    public float manaRestored;

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

    // Restore the character's mana
    private void RestoreMana(PlayerManager pm) {
        stats = pm.stats;

        // Actually update health
        if(stats != null) {
            stats.RestoreMana(manaRestored);
            Debug.Log("Mana Restored: " + manaRestored);
            CleanupManaPotion();
        } else {
            Debug.Log("Unable to find reference to PlayerStats from PlayerManager.");
        }
        
    }

    // Destroy this GameObject. Signal the PickupSpawner.
    private void CleanupManaPotion() {
        Destroy(gameObject);
        Debug.Log("Destroyed.");
    }

    // Handle collisions
    private void OnTriggerEnter(Collider other) {
        go = other.gameObject;
        pm = go.GetComponent<PlayerManager>();

        if(pm != null) {
            // collision with a player character
            Debug.Log("Collision with player");
            RestoreMana(pm);
        } else {
            // otherwise
            Debug.Log("Collision with other");
        }
    }
}
