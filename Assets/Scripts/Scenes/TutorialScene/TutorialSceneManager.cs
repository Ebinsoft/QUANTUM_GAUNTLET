using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialSceneManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    public GameObject stage;
    private PlayerInput playerInput;
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
        player.transform.position = stage.GetComponent<SpawnPoints>().GetSpawnPoint();
    }

    private void SceneSetup()
    {
        var versusInfo = GameManager.instance.versusInfo;
        versusInfo.playerList = new List<GameObject>();
        GameManager.instance.versusInfo.playerList = new List<GameObject>();
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int playerID = playerInput.playerIndex;
        Debug.Log(playerInput.devices[0]);
        // add to reference of cursor objects
        GameManager.instance.versusInfo.playerList.Add(playerInput.gameObject);
    }
}
