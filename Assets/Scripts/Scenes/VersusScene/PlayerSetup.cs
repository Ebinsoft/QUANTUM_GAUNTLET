using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerSetup : MonoBehaviour
{
    /*The idea behind this is to set up the Player prefab for stuff dependent on the scene. This 
    currently only does this for the Versus scene but I'm thinking we should have this do stuff
    depending on potential future "modes" */

    private PlayerInput playerInput;
    private CharacterController characterController;
    private PlayerManager player;
    private AIManager ai;
    private NavMeshAgent navAgent;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterController = GetComponent<CharacterController>();
        player = GetComponent<PlayerManager>();

        Setup();
    }

    void Start()
    {

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
        PlayerSetting ps = versusInfo.GetPlayer(player.playerID);
        // TODO: Update skin once we know how to do that
        // Update our player with their versusInfo settings
        gameObject.name = ps.playerName;
        gameObject.tag = ps.team.teamName;

        if (ps.playerType == PlayerType.Robot)
        {
            ChangeToAI();
        }

        // lazily put characters in spots
        characterController.enabled = false;
        transform.position = new Vector3(player.playerID, 0.5f, player.playerID);
        characterController.enabled = true;

    }

    private void ChangeToAI()
    {
        ai = gameObject.AddComponent<AIManager>();
        // disable playerInput so we won't be controlled by auto-assigned devices
        playerInput.enabled = false;
        // navAgent = gameObject.AddComponent<NavMeshAgent>();
        // navAgent.speed = player.playerSpeed;
        // navAgent.angularSpeed = player.rotationSpeed;
    }

}
