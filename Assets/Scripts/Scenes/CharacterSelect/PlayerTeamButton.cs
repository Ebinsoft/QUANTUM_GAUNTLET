using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTeamButton : MonoBehaviour
{
    private TextMeshPro text;
    private SpriteRenderer sprite;
    private VersusInfo versusInfo;
    private int playerID;
    private Animator anim;

    void Start()
    {
        text = transform.Find("Text").GetComponent<TextMeshPro>();
        sprite = GetComponent<SpriteRenderer>();
        playerID = transform.parent.GetComponent<PlayerPanel>().playerID;
        versusInfo = GameManager.instance.versusInfo;
        anim = GetComponent<Animator>();

        RedrawButton();
    }

    public void Click()
    {
        anim.SetTrigger("Click");

        PlayerSetting ps = versusInfo.GetPlayer(playerID);
        int numTeams = Enum.GetNames(typeof(TeamID)).Length;
        ps.team.teamID = (TeamID)((((int)ps.team.teamID) + 1) % numTeams);

        RedrawButton();
    }

    private void RedrawButton()
    {
        PlayerSetting ps = versusInfo.GetPlayer(playerID);
        sprite.color = ps.team.teamColor;
        text.text = ps.team.teamName;
    }
}
