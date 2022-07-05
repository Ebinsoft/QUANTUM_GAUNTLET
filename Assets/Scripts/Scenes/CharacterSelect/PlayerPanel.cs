using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        if (GameManager.instance.versusInfo.playerSettings.Select(c => c.playerIndex).Contains(playerIndex))
        {
            PlayerSetting ps = GameManager.instance.versusInfo.playerSettings.First(c => c.playerIndex == playerIndex);
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
