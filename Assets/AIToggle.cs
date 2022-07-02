using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIToggle : MonoBehaviour
{
    private TextMesh buttonText;
    private MeshRenderer button;
    private PlayerPanel playerPanel;
    private VersusInfo versusInfo;
    private bool isAIOn = false;
    public CharacterSelectManager cs;
    public Material offMaterial;
    public Material onMaterial;
    // Start is called before the first frame update
    void Start()
    {
        buttonText = transform.Find("Text").GetComponent<TextMesh>();
        button = transform.Find("Button").GetComponent<MeshRenderer>();
        playerPanel = transform.parent.GetComponent<PlayerPanel>();
        versusInfo = GameManager.instance.versusInfo;
    }

    private void AddAI()
    {
        button.material = onMaterial;
        buttonText.text = "REMOVE AI OPPONENT";

        PlayerSetting ps = new PlayerSetting
        {
            playerName = "Player " + (playerPanel.playerIndex + 1),
            playerIndex = playerPanel.playerIndex,
            device = null,
            deviceString = "none",
            // TODO: Fix this - Hard-coding this until we can have players choose AI characters
            characterName = "Edmond",
            playerType = "Robot",
            team = new Team("Team " + (playerPanel.playerIndex + 1))
        };
        versusInfo.AddPlayer(ps);
        // if an active character is on this slot, kill them
        cs.DestroyCursor(playerPanel.playerIndex);

    }

    private void RemoveAI()
    {
        button.material = offMaterial;
        buttonText.text = "TURN ON AI OPPONENT";

        versusInfo.RemovePlayer(playerPanel.playerIndex);
    }

    public void setDefault()
    {
        isAIOn = false;
        button.material = offMaterial;
        buttonText.text = "TURN ON AI OPPONENT";
    }

    public void ToggleAI()
    {
        isAIOn = !isAIOn;
        if (isAIOn)
        {
            AddAI();
        }
        else
        {
            RemoveAI();
        }

    }
}
