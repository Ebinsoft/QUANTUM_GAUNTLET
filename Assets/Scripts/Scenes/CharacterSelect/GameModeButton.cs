using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameModeButton : MonoBehaviour, IBasicButton
{
    private TextMeshPro modeText;
    private VersusInfo versusInfo;
    private Animator anim;

    void Start()
    {
        modeText = transform.Find("Mode Text").GetComponent<TextMeshPro>();
        versusInfo = GameManager.instance.versusInfo;
        anim = GetComponent<Animator>();

        SetModeText();
    }

    private void SetModeText()
    {
        switch (versusInfo.gameType)
        {
            case GameMode.FFA:
                modeText.text = "FREE-FOR-ALL";
                break;

            case GameMode.Team:
                modeText.text = "TEAM BATTLE";
                break;
        }
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

        switch (versusInfo.gameType)
        {
            case GameMode.FFA:
                versusInfo.gameType = GameMode.Team;
                break;

            case GameMode.Team:
                versusInfo.gameType = GameMode.FFA;
                versusInfo.ResetPlayerTeams();
                break;
        }
        SetModeText();
    }
}
