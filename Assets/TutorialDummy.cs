using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDummy : MonoBehaviour
{
    public GameObject collectible;
    public int collectiblesRemaining = 0;
    void Start()
    {
        TutorialSceneManager.instance.SpawnRandomCollectibles(5);
    }

    void Update()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Le trigger");
    }

    [System.Serializable]
    public class TutorialQuest
    {
        public string questText;

    }

}
