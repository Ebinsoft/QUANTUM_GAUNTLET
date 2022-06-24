using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectManager : MonoBehaviour
{

    private void Awake()
    {
        // reset versusInfo 
        VersusInfo versusInfo = GameManager.instance.versusInfo;
        // versusInfo.numPlayers = 0;
        // versusInfo.playerSettings = new List<PlayerSetting>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayerJoined()
    {
        Debug.Log("poggers");
    }
}
