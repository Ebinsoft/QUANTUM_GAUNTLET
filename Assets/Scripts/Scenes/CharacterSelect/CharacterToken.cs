using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterToken : MonoBehaviour
{
    private int playerID;

    public SpriteRenderer sprite;
    private TextMeshPro label;
    private VersusInfo versusInfo;

    void Awake()
    {
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        label = transform.Find("Label").GetComponent<TextMeshPro>();
        versusInfo = GameManager.instance.versusInfo;
    }

    void Update()
    {
        sprite.color = versusInfo.GetPlayer(playerID).team.teamColor;
    }

    public void SetPlayer(int playerID)
    {
        this.playerID = playerID;

        bool isCPU = versusInfo.GetPlayer(playerID).playerType == PlayerType.Robot;

        if (isCPU)
        {
            label.text = "CPU" + (playerID + 1);
        }
        else
        {
            label.text = "P" + (playerID + 1);
        }

        Hide();
    }

    public void Hide()
    {
        sprite.enabled = false;
        label.enabled = false;
    }

    public void Show()
    {
        sprite.enabled = true;
        label.enabled = true;
    }
}
