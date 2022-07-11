using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerPanel : MonoBehaviour
{
    private TextMeshPro text;
    private SpriteRenderer sprite;
    private GameObject addCpuButton;
    private GameObject playerTypeButton;
    private GameObject teamButton;
    private VersusInfo versusInfo;
    private CharacterSelectManager cs;

    public int playerID;

    void Start()
    {
        text = transform.Find("Prompt Text").GetComponent<TextMeshPro>();
        sprite = GetComponent<SpriteRenderer>();
        addCpuButton = transform.Find("Add CPU Button").gameObject;
        playerTypeButton = transform.Find("Player Type Button").gameObject;
        teamButton = transform.Find("Team").gameObject;
        cs = GameObject.Find("CharacterSelectManager").GetComponent<CharacterSelectManager>();

        versusInfo = GameManager.instance.versusInfo;
    }

    void Update()
    {
        PlayerSetting ps = versusInfo.playerSettings[playerID];

        if (ps != null && ps.playerType != PlayerType.None)
        {
            text.enabled = false;
            sprite.color = ps.team.teamColor;
            addCpuButton.SetActive(false);
            playerTypeButton.SetActive(true);

            if (versusInfo.gameType == GameMode.Team)
            {
                teamButton.SetActive(true);
            }
            else
            {
                teamButton.SetActive(false);
            }
        }
        else
        {
            text.enabled = true;
            sprite.color = Color.gray;
            addCpuButton.SetActive(true);
            playerTypeButton.SetActive(false);
            teamButton.SetActive(false);
        }


    }
    public void SetToCPU()
    {
        PlayerSetting ps = new PlayerSetting
        {
            playerID = playerID,
            playerName = "CPU " + (playerID + 1),
            device = null,
            deviceString = "none",
            // TODO: Fix this - Hard-coding this until we can have players choose AI characters
            character = Character.Edmond,
            playerType = PlayerType.Robot,
            team = new Team((TeamID)playerID)
        };
        versusInfo.AddPlayer(ps);
        // if an active character is on this slot, kill them
        var activeCursor = cs.playerList
            .FirstOrDefault((c => c.GetComponent<PlayerInput>().playerIndex == playerID));
        if (activeCursor != null)
        {
            cs.DestroyCursor(playerID);
        }
    }
}
