using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerTypeButton : MonoBehaviour, IBasicButton
{
    private TextMeshPro text;
    private PlayerPanel playerPanel;
    private Animator anim;
    private VersusInfo versusInfo;

    void Start()
    {
        text = transform.Find("Text").GetComponent<TextMeshPro>();
        playerPanel = transform.parent.GetComponent<PlayerPanel>();
        anim = GetComponent<Animator>();
        versusInfo = GameManager.instance.versusInfo;
    }

    void Update()
    {
        int playerID = playerPanel.playerID;
        PlayerSetting ps = versusInfo.GetPlayer(playerID);

        switch (ps.playerType)
        {
            case PlayerType.Human:
                text.text = "HUMAN";
                break;

            case PlayerType.Robot:
                text.text = "CPU";
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

        int playerID = playerPanel.playerID;
        PlayerSetting ps = versusInfo.GetPlayer(playerID);

        switch (ps.playerType)
        {
            case PlayerType.Human:
                playerPanel.SetToCPU();
                break;

            case PlayerType.Robot:
                versusInfo.RemovePlayer(playerID);
                playerPanel.RemoveCPU();
                break;
        }
    }
}
