using UnityEngine;

public class AddCpuButton : MonoBehaviour
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

    public void Click()
    {
        anim.SetTrigger("Click");
        playerPanel.SetToCPU();
    }
}