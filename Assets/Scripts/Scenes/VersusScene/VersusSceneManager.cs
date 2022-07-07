using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Cinemachine;
using System;

public class VersusSceneManager : MonoBehaviour
{
    public static VersusSceneManager instance;
    public PlayerInputManager playerInputManager;
    public List<GameObject> playerList;
    public PauseMenu gameOverMenu;
    public CinemachineTargetGroup playerTargetGroup;
    public UnityEngine.Object playerHUDPrefab;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        var versusInfo = GameManager.instance.versusInfo;
        int numPlayers = versusInfo.numPlayers;
        isGameOver = false;

        for (int i = 0; i < versusInfo.playerSettings.Count; i++)
        {
            PlayerSetting ps = versusInfo.playerSettings[i];
            // TODO: Remove the need to ever gather playerIndex and just generate it here
            ps.playerIndex = i;

            CharacterData c = GameManager.instance.roster.GetCharacter(ps.characterName);
            PlayerManager playerManager = null;

            if (ps.playerType == "Human")
            {
                playerInputManager.playerPrefab = c.characterPrefab;
                if (ps.device != null)
                {

                    var playerInput = playerInputManager.JoinPlayer(i, -1, null, ps.device);
                    playerManager = playerInput.gameObject.GetComponent<PlayerManager>();
                }
                else
                {
                    // This is for simple debugging directly from Versus scene
                    var playerInput = playerInputManager.JoinPlayer(i, -1, null);
                    playerManager = playerInput.gameObject.GetComponent<PlayerManager>();
                }

            }

            else if (ps.playerType == "Robot")
            {
                playerManager = ((GameObject)Instantiate(c.characterPrefab)).GetComponent<PlayerManager>();
            }

            CreatePlayerHUD(playerManager, ps);
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
        // subscribe to player's event for their death for removal from list
        playerInput.gameObject.GetComponent<PlayerStats>().onPlayerLose += onPlayerLose;
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

    public void onPlayerLose(GameObject player)
    {
        playerList.Remove(player);
        playerTargetGroup.RemoveMember(player.transform);
    }

    private void CreatePlayerHUD(PlayerManager playerManager, PlayerSetting playerSetting)
    {
        GameObject hudObj = (GameObject)Instantiate(playerHUDPrefab);
        GameObject canvas = GameObject.Find("Canvas");
        hudObj.transform.SetParent(canvas.transform);

        PlayerHUD hud = hudObj.GetComponent<PlayerHUD>();
        hud.SetPlayer(playerManager, playerSetting);
    }
}