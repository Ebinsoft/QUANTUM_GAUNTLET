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
    public PauseMenu gameOverMenu;
    public CinemachineTargetGroup playerTargetGroup;
    public UnityEngine.Object playerHUDPrefab;
    // should generate these dynamically once we add stage loading
    private SpawnPoints spawnPoints;
    private bool isGameOver = false;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        GameManager.instance.versusInfo.playerList = new List<GameObject>();
        GameObject stage = SpawnStage();
        spawnPoints = stage.GetComponent<SpawnPoints>();
        List<Vector3> startingSpawns = spawnPoints.GetMututallyExclusiveSpawnPoints(GameManager.instance.versusInfo.numPlayers);
        foreach (PlayerSetting ps in GameManager.instance.versusInfo.GetActivePlayers())
        {
            CharacterData c = GameManager.instance.roster.GetCharacter(ps.character);
            playerInputManager.playerPrefab = c.characterPrefab;
            // set player ID
            c.characterPrefab.GetComponent<PlayerManager>().playerID = ps.playerID;

            PlayerManager playerManager = null;
            switch (ps.playerType)
            {
                case PlayerType.Human:
                    if (ps.device != null)
                    {
                        var playerInput = playerInputManager.JoinPlayer(-1, -1, null, ps.device);
                        playerManager = playerInput.gameObject.GetComponent<PlayerManager>();
                    }
                    else
                    {
                        // This is for simple debugging directly from Versus scene
                        var playerInput = playerInputManager.JoinPlayer();
                        playerManager = playerInput.gameObject.GetComponent<PlayerManager>();
                    }
                    break;
                case PlayerType.Robot:
                    playerManager = ((GameObject)Instantiate(c.characterPrefab)).GetComponent<PlayerManager>();
                    break;
            }

            playerManager.Teleport(startingSpawns[0]);
            startingSpawns.RemoveAt(0);
            HookUpPlayer(playerManager.gameObject);
            CreatePlayerHUD(playerManager, ps);
            c.characterPrefab.GetComponent<PlayerManager>().playerID = 0;
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

    // Will load the stage from VersusInfo, handle random, and return the instantiated object
    public GameObject SpawnStage()
    {
        StageData sd;
        var vi = GameManager.instance.versusInfo;
        if (vi.stage == Stage.Random)
        {
            sd = GameManager.instance.roster.GetRandomStage();
        }
        else
        {
            sd = GameManager.instance.roster.GetStage(vi.stage);
        }

        GameObject stage = Instantiate(sd.stagePrefab);

        return stage;
    }

    public void HookUpPlayer(GameObject playerObject)
    {
        // whenever a player joins, get a reference to their GameObject 
        GameManager.instance.versusInfo.playerList.Add(playerObject);
        // subscribe to player's event for their death for removal from list
        PlayerStats playerStats = playerObject.GetComponent<PlayerStats>();
        playerStats.onPlayerSpawn += onPlayerSpawn;
        playerStats.onPlayerLose += onPlayerLose;
        playerStats.onPlayerDie += onPlayerDie;
        // Add player to tracked objects of camera
        playerTargetGroup.AddMember(playerObject.transform, 1f, 2f);
    }

    public bool CheckIfGameOver()
    {
        int numUniqueTeamsStillAlive =
            GameManager.instance.versusInfo.playerList.Where(c => c.GetComponent<PlayerManager>().stats.lives > 0)
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

    public void onPlayerSpawn(GameObject player)
    {
        PlayerManager pm = player.GetComponent<PlayerManager>();
        pm.stats.lives = GameManager.instance.versusInfo.numLives;
    }

    public void onPlayerLose(GameObject player)
    {
        GameManager.instance.versusInfo.playerList.Remove(player);
        playerTargetGroup.RemoveMember(player.transform);
    }

    public void onPlayerDie(GameObject player)
    {
        PlayerManager pm = player.GetComponent<PlayerManager>();
        pm.Teleport(spawnPoints.GetSpawnPoint());
    }

    private void CreatePlayerHUD(PlayerManager playerManager, PlayerSetting playerSetting)
    {
        GameObject hudObj = (GameObject)Instantiate(playerHUDPrefab);
        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        hudObj.transform.SetParent(canvas.transform);

        PlayerHUD hud = hudObj.GetComponent<PlayerHUD>();
        hud.SetPlayer(playerManager, playerSetting);
    }
}