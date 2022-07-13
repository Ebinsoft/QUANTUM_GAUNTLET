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

    void IBasicButton.Click()
    {
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
                break;
        }
    }
}
