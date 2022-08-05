using UnityEngine;

public class AddCpuButton : MonoBehaviour, IBasicButton
{
    private Animator anim;
    private PlayerPanel playerPanel;
    private VersusInfo versusInfo;

    void Start()
    {
        playerPanel = transform.parent.GetComponent<PlayerPanel>();
        anim = GetComponent<Animator>();
        versusInfo = GameManager.instance.versusInfo;
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
        playerPanel.SetToCPU();
    }
}