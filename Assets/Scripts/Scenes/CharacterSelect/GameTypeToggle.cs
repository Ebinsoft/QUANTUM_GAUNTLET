using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTypeToggle : MonoBehaviour
{
    public bool isFFA = true;
    private TextMesh modeText;
    private MeshRenderer modeBox;
    private GameObject teamButtons;
    public Material ffaMaterial;
    public Material teamMaterial;


    void Start()
    {
        modeText = transform.Find("ToggleText").GetComponent<TextMesh>();
        modeBox = transform.Find("ToggleBox").GetComponent<MeshRenderer>();
        teamButtons = transform.Find("TeamButtons").gameObject;
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
        // reset everyone to separate teams
        GameManager.instance.versusInfo.ResetPlayerTeams();
        teamButtons.SetActive(false);

        modeText.text = "FREE FOR ALL";
        modeBox.material = ffaMaterial;
    }

    private void SetTeamMode()
    {
        teamButtons.SetActive(true);
        modeText.text = "TEAM BATTLE";
        modeBox.material = teamMaterial;
    }
}
