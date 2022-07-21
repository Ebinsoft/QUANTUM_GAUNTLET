using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialSceneManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    public GameObject stage;
    private PlayerInput playerInput;

    public UnityEngine.Object playerHUDPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    void Start()
    {
        SceneSetup();
        playerInput = playerInputManager.JoinPlayer();
        var player = playerInput.GetComponent<PlayerManager>();
        player.Teleport(stage.GetComponent<SpawnPoints>().GetSpawnPoint());
        PlayerSetting ps = CreatePlayerSetting(player);
        CreatePlayerHUD(player, ps);

    }

    private void SceneSetup()
    {
        var versusInfo = GameManager.instance.versusInfo;
        versusInfo.playerList = new List<GameObject>();
        GameManager.instance.versusInfo.playerList = new List<GameObject>();
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

    private void CreatePlayerHUD(PlayerManager playerManager, PlayerSetting playerSetting)
    {
        GameObject hudObj = (GameObject)Instantiate(playerHUDPrefab);
        GameObject canvas = GameObject.Find("Canvas");
        hudObj.transform.SetParent(canvas.transform);

        PlayerHUD hud = hudObj.GetComponent<PlayerHUD>();
        hud.SetPlayer(playerManager, playerSetting);
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex;
        // add to reference of cursor objects
        GameManager.instance.versusInfo.playerList.Add(playerInput.gameObject);
    }
}
