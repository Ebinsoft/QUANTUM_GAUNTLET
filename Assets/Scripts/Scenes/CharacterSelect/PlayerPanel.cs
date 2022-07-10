using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPanel : MonoBehaviour
{
    private TextMesh text;
    public int playerID;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.Find("Text").GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerSetting ps = GameManager.instance.versusInfo.playerSettings[playerID];
        if (ps != null && ps.playerType != PlayerType.None)
        {
            string characterField = string.IsNullOrEmpty(ps.characterName) ? "CHOOSE CHARACTER" : ps.characterName;
            text.text = ps.playerName + "\n" + characterField + "\n" + ps.playerType;
            text.color = ps.team.teamColor;
        }
        else
        {
            text.text = "PRESS START(Gamepad)\n or ENTER(keyboard)\n to join!";
            text.color = Color.black;
        }


    }
}
