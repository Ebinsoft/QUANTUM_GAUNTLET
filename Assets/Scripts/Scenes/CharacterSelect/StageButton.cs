using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StageButton : MonoBehaviour, IBasicButton
{
    public enum StageButtonType
    {
        Next,
        Previous
    }
    public StageButtonType buttonType;
    private TextMeshPro currentStage;
    private VersusInfo versusInfo;
    private Animator anim;
    private int numStages;

    void Start()
    {
        versusInfo = GameManager.instance.versusInfo;
        currentStage = transform.parent.Find("Current Stage").GetComponent<TextMeshPro>();
        anim = GetComponent<Animator>();
        numStages = Enum.GetNames(typeof(Stage)).Length;
        currentStage.text = versusInfo.stage.ToString();
    }

    void IBasicButton.HoverEnter()
    {
        anim.SetBool("Hovering", true);
    }

    void IBasicButton.HoverExit()
    {
        anim.SetBool("Hovering", false);
    }

    void IBasicButton.Click()
    {
        anim.SetTrigger("Click");

        switch (buttonType)
        {
            case StageButtonType.Next:
                if((int)versusInfo.stage == numStages - 1)
                {
                    versusInfo.stage = (Stage)0;
                }
                else
                {
                    versusInfo.stage++;
                }
                break;

            case StageButtonType.Previous:
                if(versusInfo.stage == 0)
                {
                    versusInfo.stage = (Stage)numStages - 1;
                }
                else{
                    versusInfo.stage--;
                }
                break;
        }

        currentStage.text = GameManager.instance.roster.GetStage(versusInfo.stage).stageName;
    }
}
