using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StockButton : MonoBehaviour, IBasicButton
{
    public enum ButtonType
    {
        Add,
        Subtract
    }
    public ButtonType buttonType;
    private TextMeshPro stockCount;
    private VersusInfo versusInfo;
    private Animator anim;

    void Start()
    {
        versusInfo = GameManager.instance.versusInfo;
        stockCount = transform.parent.Find("Stock Count").GetComponent<TextMeshPro>();
        anim = GetComponent<Animator>();
        // update stock to our default stock numberin CS on load
        versusInfo.numLives = int.Parse(stockCount.text);
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

        int newLives;
        switch (buttonType)
        {
            case ButtonType.Add:
                newLives = versusInfo.numLives + 1;
                versusInfo.numLives = Mathf.Min(10, newLives);
                break;

            case ButtonType.Subtract:
                newLives = versusInfo.numLives - 1;
                versusInfo.numLives = Mathf.Max(1, newLives);
                break;
        }

        stockCount.text = versusInfo.numLives.ToString();
    }
}
