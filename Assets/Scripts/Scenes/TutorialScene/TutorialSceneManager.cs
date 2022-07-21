using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class TutorialSceneManager : MonoBehaviour
{
    public static TutorialSceneManager instance;
    private PlayerInputManager playerInputManager;
    public GameObject stage;
    private PlayerInput playerInput;

    public UnityEngine.Object playerHUDPrefab;
    public CinemachineTargetGroup playerTargetGroup;
    public GameObject collectible;
    public int collectiblesRemaining = 0;
    public GameObject trainingDummy;
    private SpawnPoints spawnPoints;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        playerInputManager = GetComponent<PlayerInputManager>();
        spawnPoints = stage.GetComponent<SpawnPoints>();
    }

    void Start()
    {
        SceneSetup();
        PlayerSetup();
    }

    private void PlayerSetup()
    {
        playerInput = playerInputManager.JoinPlayer();
        var player = playerInput.GetComponent<PlayerManager>();
        player.Teleport(new Vector3(0f, 2f, 0f));
        PlayerSetting ps = CreatePlayerSetting(player);
        CreateTutorialPlayerHUD(player, ps);
        player.gameObject.GetComponent<PlayerStats>().onPlayerDie += onPlayerDie;
        playerTargetGroup.AddMember(player.gameObject.transform, 1f, 2f);
    }

    private void SceneSetup()
    {
        GameManager.instance.versusInfo.playerList = new List<GameObject>();

        playerTargetGroup.AddMember(trainingDummy.transform, .5f, 2f);
    }

    // hard-code a PlayerSetting for nwo for the UI
    private PlayerSetting CreatePlayerSetting(PlayerManager player)
    {

        PlayerSetting ps = new PlayerSetting
        {
            playerID = player.playerID,
            playerName = "Player " + (player.playerID + 1),
            device = playerInput.devices[0],
            deviceString = playerInput.devices[0].ToString(),
            playerType = PlayerType.Human,
            team = new Team((TeamID)player.playerID),
            character = Character.None
        };
        return ps;
    }

    public void SpawnRandomCollectibles(int num)
    {
        collectiblesRemaining = num;
        List<Vector3> points = spawnPoints.GetMututallyExclusiveSpawnPoints(num);
        foreach (Vector3 point in points)
        {
            Instantiate(collectible, point, collectible.transform.rotation);
        }
    }

    public void SpawnCollectibleAtPoint(Vector3 p)
    {
        collectiblesRemaining++;
        Instantiate(collectible, p, collectible.transform.rotation);
    }

    private void CreateTutorialPlayerHUD(PlayerManager playerManager, PlayerSetting playerSetting)
    {
        GameObject hudObj = (GameObject)Instantiate(playerHUDPrefab);
        GameObject canvas = GameObject.Find("Canvas");
        hudObj.transform.SetParent(canvas.transform);

        PlayerHUD hud = hudObj.GetComponent<PlayerHUD>();
        hud.SetPlayer(playerManager, playerSetting);
    }

    public void onPlayerDie(GameObject player)
    {
        PlayerManager pm = player.GetComponent<PlayerManager>();
        pm.Teleport(spawnPoints.GetSpawnPoint());
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex;
        // add to reference of cursor objects
        GameManager.instance.versusInfo.playerList.Add(playerInput.gameObject);
    }
}
