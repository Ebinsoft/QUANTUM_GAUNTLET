using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviour
{
    /*The idea behind this is to set up the Player prefab for stuff dependent on the scene. This 
    currently only does this for the Versus scene but I'm thinking we should have this do stuff
    depending on potential future "modes" */

    private PlayerInput playerInput;
    private PlayerSetting playerSetting;
    private CharacterController characterController;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        // if currently in Versus mode maybe? Like have GameManager have current scene or something
        SetupVersus();
    }

    private void SetupVersus()
    {
        // Look up ourselves in the versus player settings
        VersusInfo versusInfo = GameManager.instance.versusInfo;
        foreach (PlayerSetting ps in versusInfo.playerSettings)
        {
            if (ps.playerIndex == playerInput.playerIndex)
            {
                playerSetting = ps;
            }
        }
        if (playerSetting == null)
        {
            Debug.LogError("No matching playerSetting found in GameManager.versusInfo.playerSettings");
        }

        else
        {
            // TODO: Update skin once we know how to do that
            // Update our player with their versusInfo settings
            gameObject.name = playerSetting.playerName;
            gameObject.tag = playerSetting.team;
            // lazily put characters in spots
            characterController.enabled = false;
            transform.position = new Vector3(playerSetting.playerIndex, 0.5f, playerSetting.playerIndex);
            characterController.enabled = true;
        }
    }

}
