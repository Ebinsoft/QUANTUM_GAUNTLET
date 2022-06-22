using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneManager : MonoBehaviour
{
    public GameObject player;
    public PlayerInputManager playerInputManager;
    // Start is called before the first frame update
    void Start()
    {
        CharacterData c = GameManager.instance.roster.GetCharacter("Edmond");
        player = c.characterPrefab;
        GameManager.instance.numPlayers = 2;
        for (int i = 0; i < GameManager.instance.numPlayers; i++)
        {
            player.name = "Player " + i;
            SpawnCharacter(c);
        }
    }

    void SpawnCharacter(CharacterData c)
    {
        playerInputManager.playerPrefab = player;
        playerInputManager.JoinPlayer();
    }
}
