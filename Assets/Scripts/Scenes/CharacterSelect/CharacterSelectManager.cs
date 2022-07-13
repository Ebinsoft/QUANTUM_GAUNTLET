using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class CharacterSelectManager : MonoBehaviour
{
    private VersusInfo versusInfo;
    public GameObject playerPanels;
    public List<GameObject> playerList;
    public List<GameObject> tokenList;

    void Start()
    {
        versusInfo = GameManager.instance.versusInfo;
        versusInfo.numPlayers = 0;
        versusInfo.playerSettings = new PlayerSetting[4];
    }

    public void DestroyCursor(int playerIndex)
    {
        GameObject cursor = playerList.First(c => c.GetComponent<PlayerInput>().playerIndex == playerIndex);
        playerList.Remove(cursor);
        Destroy(cursor);
    }

    public void DestroyToken(int playerID)
    {
        GameObject tokenObj = tokenList.FirstOrDefault(c => c.GetComponent<CharacterToken>().playerID == playerID);


        if (tokenObj != null)
        {
            tokenList.Remove(tokenObj);
            // cleanup any characterBox lists of this token so it doesn't have dead objects
            tokenObj.GetComponent<CharacterToken>().CleanUp();
            Destroy(tokenObj);
        }
    }

    public void AddToken(GameObject token)
    {
        tokenList.Add(token);
    }

    void OnPlayerJoined(PlayerInput playerInput)
    {
        // set his PlayerPanel to default values in case of re-joining
        PlayerPanel pp = playerPanels.transform
            .Find("Player " + (playerInput.playerIndex + 1))
            .GetComponent<PlayerPanel>();

        PlayerSetting ps = new PlayerSetting
        {
            playerID = pp.playerID,
            playerName = "Player " + (pp.playerID + 1),
            device = playerInput.devices[0],
            deviceString = playerInput.devices[0].ToString(),
            playerType = PlayerType.Human,
            team = new Team((TeamID)pp.playerID),
            character = Character.None
        };

        versusInfo.AddPlayer(ps);
        // add to reference of cursor objects
        playerList.Add(playerInput.gameObject);
    }
}
