using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VersusSceneManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    // Start is called before the first frame update
    void Start()
    {
        var versusInfo = GameManager.instance.versusInfo;
        int numPlayers = versusInfo.numPlayers;

        foreach (PlayerSetting ps in versusInfo.playerSettings)
        {
            CharacterData c = GameManager.instance.roster.GetCharacter(ps.characterName);
            playerInputManager.playerPrefab = c.characterPrefab;
            playerInputManager.JoinPlayer(ps.playerIndex, -1, null, ps.device);
        }
    }
}