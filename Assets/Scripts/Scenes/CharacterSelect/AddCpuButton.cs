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

    void IBasicButton.Click()
    {
        anim.SetTrigger("Click");
        playerPanel.SetToCPU();
    }
}