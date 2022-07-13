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

    void IBasicButton.Click()
    {
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
