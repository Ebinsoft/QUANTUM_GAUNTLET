using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour, IBasicButton
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
        GameManager.instance.TransitionToScene("MainMenu");
    }
}