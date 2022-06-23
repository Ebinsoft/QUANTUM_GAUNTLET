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
        CharacterData c = GameManager.instance.roster.GetCharacter("Edmond");
        player = c.characterPrefab;
        for (int i = 0; i < numPlayers; i++)
        {
            player.name = "Player " + i;
            player.tag = "Team " + (i + 1);
            player.transform.position = new Vector3(i, 0.5f, i);
            SpawnCharacter(c);
        }
    }

    void SpawnCharacter(CharacterData c)
    {
        playerInputManager.playerPrefab = player;
        playerInputManager.JoinPlayer();
    }
}
