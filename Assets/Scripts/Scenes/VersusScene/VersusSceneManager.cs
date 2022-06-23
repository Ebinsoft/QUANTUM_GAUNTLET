using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VersusSceneManager : MonoBehaviour
{
    public GameObject player;
    public PlayerInputManager playerInputManager;
    // Start is called before the first frame update
    void Start()
    {
        var versusInfo = GameManager.instance.versusInfo;
        int numPlayers = versusInfo.numPlayers;

        foreach (PlayerSettings ps in versusInfo.playerSettings)
        {
            CharacterData c = GameManager.instance.roster.GetCharacter(ps.characterName);
            player = c.characterPrefab;
            player.name = ps.playerName;
            player.tag = ps.team;
            player.transform.position = new Vector3(ps.playerIndex, 0.5f, ps.playerIndex);
            playerInputManager.playerPrefab = player;
            playerInputManager.JoinPlayer();
        }
    }
}