using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTeamButton : MonoBehaviour, IBasicButton
{
    private TextMeshPro text;
    private SpriteRenderer sprite;
    private VersusInfo versusInfo;
    private int playerID;
    private Animator anim;

    void Awake()
    {
        text = transform.Find("Text").GetComponent<TextMeshPro>();
        sprite = GetComponent<SpriteRenderer>();
        playerID = transform.parent.GetComponent<PlayerPanel>().playerID;
        versusInfo = GameManager.instance.versusInfo;
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        RedrawButton();
    }

    void IBasicButton.HoverEnter()
    {
        Sound s = AudioManager.UISounds[UISound.CSHover];
        AudioManager.Play2D(s);
        anim.SetBool("Hovering", true);
    }

    void IBasicButton.HoverExit()
    {
        anim.SetBool("Hovering", false);
    }

    void IBasicButton.Click()
    {
        Sound s = AudioManager.UISounds[UISound.CSClick];
        AudioManager.Play2D(s);
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
