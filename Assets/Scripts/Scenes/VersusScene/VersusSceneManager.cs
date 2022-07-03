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

        foreach (PlayerSetting ps in versusInfo.playerSettings.OrderBy(c => c.playerIndex))
        {
            CharacterData c = GameManager.instance.roster.GetCharacter(ps.characterName);
            if (ps.playerType == "Human")
            {
                playerInputManager.playerPrefab = c.characterPrefab;
                if (ps.device != null)
                {

                    playerInputManager.JoinPlayer(ps.playerIndex, -1, null, ps.device);
                }
                else
                {
                    // This is for simple debugging directly from Versus scene
                    playerInputManager.JoinPlayer(ps.playerIndex, -1, null);
                }

            }

            else if (ps.playerType == "Robot")
            {
                Instantiate(c.characterPrefab);
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
        PlayerSetting ps = GameManager.instance.versusInfo.GetPlayer(playerInput.playerIndex);
        // whenever a player joins, get a reference to their GameObject 
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