using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class CharacterSelectManager : MonoBehaviour
{
    private VersusInfo versusInfo;
    public GameObject characterSelectScreen;
    private GameObject playerPanels;
    public List<GameObject> playerList;
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
        versusInfo.ResetPlayers();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DestroyCursor(int playerIndex)
    {
        GameObject cursor = playerList.First(c => c.GetComponent<PlayerInput>().playerIndex == playerIndex);
        playerList.Remove(cursor);
        Destroy(cursor);
    }
    void OnPlayerJoined(PlayerInput playerInput)
    {
        // set his PlayerPanel to default values in case of re-joining
        PlayerPanel pp = playerPanels.transform.Find("Player" + playerInput.playerIndex).GetComponent<PlayerPanel>();
        pp.transform.Find("AI Toggle").GetComponent<AIToggle>().setDefault();

        PlayerSetting ps = new PlayerSetting
        {
            playerID = pp.playerID,
            playerName = "Player " + (pp.playerID + 1),
            playerIndex = playerInput.playerIndex,
            device = playerInput.devices[0],
            deviceString = playerInput.devices[0].ToString(),
            playerType = PlayerType.Human,
            team = new Team("Team " + (pp.playerID + 1))
        };

        versusInfo.AddPlayer(ps);
        // add to reference of cursor objects
        playerList.Add(playerInput.gameObject);


    }

    void OnPlayerLeft(PlayerInput playerInput)
    {
        // versusInfo.RemovePlayer(playerInput.playerIndex);
    }
}
