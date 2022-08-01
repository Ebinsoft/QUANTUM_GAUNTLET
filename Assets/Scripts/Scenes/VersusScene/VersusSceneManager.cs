using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Cinemachine;
using TMPro;
using System;

public class VersusSceneManager : MonoBehaviour
{
    public static VersusSceneManager instance;
    public PlayerInputManager playerInputManager;
    public PauseMenu gameOverMenu;
    public GameObject GameOverSplash;
    public GameObject WinText;
    public CinemachineTargetGroup playerTargetGroup;
    public CinemachineVirtualCamera cinemachineCamera;
    public CinemachineVirtualCamera gameEndCamera;
    public UnityEngine.Object playerHUDPrefab;
    // should generate these dynamically once we add stage loading
    private SpawnPoints spawnPoints;
    private GameObject killPlane;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.versusInfo.playerList = new List<GameObject>();
        GameObject stage = SpawnStage();
        killPlane = stage.transform.Find("KillPlane").gameObject;
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
        playerStats.onPlayerDie += onPlayerDie;
        playerStats.onPlayerRespawn += onPlayerRespawn;
        playerStats.onPlayerLose += onPlayerLose;
        // Add player to tracked objects of camera
        playerTargetGroup.AddMember(playerObject.transform, 1f, 2f);
    }

    public bool CheckIfGameOver()
    {
        int numUniqueTeamsStillAlive =
            GameManager.instance.versusInfo.playerList.Where(c => c.GetComponent<PlayerManager>().stats.lives > 0)
            .GroupBy(c => c.tag)
            .Count();

        return numUniqueTeamsStillAlive <= 1;
    }

    public void FinishGame()
    {
        StartCoroutine(GameOutro());
    }

    // This will immediately print game on the screen and slow the game down for a bit for cool effect
    public IEnumerator GameOutro()
    {
        GameOverSplash.SetActive(true);
        Time.timeScale = .25f;
        yield return new WaitForSeconds(1f);
        GameOverSplash.SetActive(false);
        Time.timeScale = 1f;
        // rotate camera and disable characters
        // also disable kill plane
        killPlane.SetActive(false);
        var playersLeft = GameManager.instance.versusInfo.playerList
        .Where(c => c.GetComponent<PlayerManager>().stats.lives > 0);
        foreach (var p in playersLeft)
        {
            var pm = p.GetComponent<PlayerManager>();
            pm.canDie = false;
            pm.triggerDisabled = true;
        }
        // get first player that's alive in the winning team so only focus on one
        var firstPlayer = playersLeft.First(c => c);

        cinemachineCamera.gameObject.SetActive(false);
        gameEndCamera.gameObject.SetActive(true);
        gameEndCamera.Follow = firstPlayer.transform;
        gameEndCamera.LookAt = firstPlayer.transform;
        // this should probably actually wait for the winning animation to finish
        yield return new WaitForSeconds(2f);

        // put winning player into victory state
        firstPlayer.GetComponent<PlayerManager>().triggerVictory = true;
        yield return new WaitForSeconds(2f);

        PlayerSetting ps = GameManager.instance.versusInfo.GetPlayer(firstPlayer.GetComponent<PlayerManager>().playerID);
        WinText.GetComponent<TextMeshProUGUI>().color = ps.team.teamColor;
        string winString = GameManager.instance.versusInfo.gameType == GameMode.FFA ? ps.playerName : ps.team.teamName + " TEAM";
        WinText.SetActive(true);
        WinText.GetComponent<TextMeshProUGUI>().SetText(winString + " WINS!");

        yield return new WaitForSeconds(2f);

        WinText.SetActive(false);
        gameOverMenu.EnablePauseMenu();
    }

    public void onPlayerSpawn(GameObject player)
    {
        PlayerManager pm = player.GetComponent<PlayerManager>();
        pm.stats.lives = GameManager.instance.versusInfo.numLives;
    }

    public void onPlayerDie(GameObject player)
    {
        if (CheckIfGameOver())
        {
            FinishGame();
        }
    }

    public void onPlayerLose(GameObject player)
    {
        GameManager.instance.versusInfo.playerList.Remove(player);
        playerTargetGroup.RemoveMember(player.transform);
    }

    public void onPlayerRespawn(GameObject player)
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