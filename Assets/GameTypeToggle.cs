using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTypeToggle : MonoBehaviour
{
    public bool isFFA = true;
    private TextMesh modeText;
    private MeshRenderer modeBox;
    public Material ffaMaterial;
    public Material teamMaterial;


    void Start()
    {
        modeText = transform.Find("ToggleText").GetComponent<TextMesh>();
        modeBox = transform.Find("ToggleBox").GetComponent<MeshRenderer>();
    }

    public void ToggleMode()
    {
        isFFA = !isFFA;
        if (isFFA)
        {
            SetFFAMode();

        }
        else
        {
            SetTeamMode();
        }
    }

    private void SetFFAMode()
    {
        modeText.text = "FREE FOR ALL";
        modeBox.material = ffaMaterial;
    }

    private void SetTeamMode()
    {
        modeText.text = "TEAM BATTLE";
        modeBox.material = teamMaterial;
    }
}
