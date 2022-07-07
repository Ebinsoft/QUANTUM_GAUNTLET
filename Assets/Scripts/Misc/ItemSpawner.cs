using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public GameObject spawnee;  // prefab to be spawned
    public float timer;         // delay between respawns (seconds)

    // public for debugging purposes
    public bool spawningItem;   // set true by spawnee's Cleanup()
    public float timeRemaining;

    private InteractableItem spawnedItem; 

    // Start is called before the first frame update
    void Start()
    {
        
        if(spawnee == null) {
            Debug.Log("ItemSpawner spawnee is set to null!");
        } else {
            SpawnItem();
        }

        spawningItem = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(spawningItem) {
            timeRemaining -= Time.deltaTime;

            if(timeRemaining <= 0) {
                SpawnItem();
            }
        }
    }

    // Make call to instantiate prefab
    public void SpawnItem() {
        GameObject go = Instantiate(spawnee, transform.position + new Vector3(0f, 1.5f, 0f), new Quaternion(0.25f, 0.5f, 0.5f, 0.0f));
        spawnedItem = go.GetComponent<InteractableItem>();

        if(spawnedItem == null) {
            Debug.Log("Could not obtain reference to InteractableItem");
        } else {
            Debug.Log("Item spawned");
            spawnedItem.spawner = this;
        }
        spawningItem = false;
    }

    // Called by the spawnee
    public void ItemTaken() {
        timeRemaining = timer;
        spawningItem = true;
    }
}
