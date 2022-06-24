using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectManager : MonoBehaviour
{
    private VersusInfo versusInfo;
    private void Awake()
    {
        // reset versusInfo 

        // versusInfo.numPlayers = 0;
        // versusInfo.playerSettings = new List<PlayerSetting>();
    }
    // Start is called before the first frame update
    void Start()
    {
        versusInfo = GameManager.instance.versusInfo;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        PlayerSetting ps = new PlayerSetting
        {
            playerIndex = playerInput.playerIndex,
            device = playerInput.devices[0],
            deviceString = playerInput.devices[0].ToString(),
            team = "FFA"
        };
        versusInfo.playerSettings.Add(ps);
    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Playing Leaving: NOT poggers");
    }
}
