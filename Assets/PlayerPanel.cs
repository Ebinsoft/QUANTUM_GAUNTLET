using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    private TextMesh text;
    public int playerIndex;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.Find("Text").GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerSetting ps = GameManager.instance.versusInfo.playerSettings[playerIndex];
        Debug.Log(ps.playerName);
        string characterField = string.IsNullOrEmpty(ps.characterName) ? "CHOOSE CHARACTER" : ps.characterName;
        text.text = ps.playerName + "\n" + characterField + "\n" + ps.playerType;
        text.color = ps.team.teamColor;

    }
}
