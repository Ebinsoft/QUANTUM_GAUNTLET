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
        playerInput = playerInputManager.JoinPlayer();
        var player = playerInput.GetComponent<PlayerManager>();
        player.transform.position = stage.GetComponent<SpawnPoints>().GetSpawnPoint();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
