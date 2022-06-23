using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VersusSceneManager : MonoBehaviour
{
    private GameObject player;
    public PlayerInputManager playerInputManager;
    // Start is called before the first frame update
    void Start()
    {
        var versusInfo = GameManager.instance.versusInfo;
        int numPlayers = versusInfo.numPlayers;

        foreach (PlayerSetting ps in versusInfo.playerSettings)
        {
            CharacterData c = GameManager.instance.roster.GetCharacter(ps.characterName);
            player = c.characterPrefab;
            playerInputManager.playerPrefab = player;
            // TODO: Manually set the InputDevice once we generate it in charSelect
            playerInputManager.JoinPlayer(ps.playerIndex, -1, null);
        }
    }
}