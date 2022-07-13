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
    }

    void IBasicButton.Click()
    {
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
