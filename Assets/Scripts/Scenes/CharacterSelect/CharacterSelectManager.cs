using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectManager : MonoBehaviour
{
    private VersusInfo versusInfo;
    public GameObject characterSelectScreen;
    private GameObject playerPanels;
    private void Awake()
    {
        // reset versusInfo 
        playerPanels = characterSelectScreen.transform.Find("PlayerPanels").gameObject;

    }
    // Start is called before the first frame update
    void Start()
    {
        versusInfo = GameManager.instance.versusInfo;
        versusInfo.numPlayers = 0;
        versusInfo.playerSettings = new List<PlayerSetting>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        versusInfo.numPlayers++;
        PlayerSetting ps = new PlayerSetting
        {
            playerName = "Player " + (playerInput.playerIndex + 1),
            playerIndex = playerInput.playerIndex,
            device = playerInput.devices[0],
            deviceString = playerInput.devices[0].ToString(),
            playerType = "Human",
            team = new Team("Team " + (playerInput.playerIndex + 1))
        };

        versusInfo.playerSettings.Add(ps);
        // enable playerPanel
        GameObject pp = playerPanels.transform.Find("Player" + playerInput.playerIndex).gameObject;
        if (pp != null)
        {
            pp.SetActive(true);
        }

    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Playing Leaving: NOT poggers");
    }
}
