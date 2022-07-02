using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Cinemachine;

public class VersusSceneManager : MonoBehaviour
{
    public PlayerInputManager playerInputManager;
    public List<GameObject> playerList;
    public PauseMenu gameOverMenu;
    public CinemachineTargetGroup playerTargetGroup;
    private bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        var versusInfo = GameManager.instance.versusInfo;
        int numPlayers = versusInfo.numPlayers;
        isGameOver = false;

        foreach (PlayerSetting ps in versusInfo.playerSettings)
        {
            if (ps.playerType == "Human")
            {
                CharacterData c = GameManager.instance.roster.GetCharacter(ps.characterName);
                playerInputManager.playerPrefab = c.characterPrefab;
                playerInputManager.JoinPlayer(ps.playerIndex, -1, null, ps.device);
            }

        }

    }

    private void Update()
    {
        if (!isGameOver && CheckIfGameOver())
        {
            isGameOver = true;
            gameOverMenu.EnableGameOver();
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // whenever a player joins, get a reference to their GameObject 
        playerInput.gameObject.name = "Player " + (playerInput.playerIndex + 1);
        playerList.Add(playerInput.gameObject);
        // Add player to tracked objects of camera
        playerTargetGroup.AddMember(playerInput.gameObject.transform, 1f, 2f);
    }

    public bool CheckIfGameOver()
    {
        int numUniqueTeamsStillAlive =
            playerList.Where(c => c.GetComponent<PlayerManager>().stats.lives > 0)
            .GroupBy(c => c.tag)
            .Count();
        if (numUniqueTeamsStillAlive > 1)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
}