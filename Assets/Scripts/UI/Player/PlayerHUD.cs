using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Material[] healthBarMaterials;
    public Material[] manaBarMaterials;

    PlayerManager player;
    PlayerSetting playerSetting;

    Image healthBar;
    Image manaBar;
    Image portraitBg;
    TMPro.TextMeshProUGUI playerID;

    void Awake()
    {
        healthBar = transform.Find("Health/Bar").GetComponent<Image>();
        manaBar = transform.Find("Mana/Bar").GetComponent<Image>();
        portraitBg = transform.Find("Portrait/BG").GetComponent<Image>();
        playerID = transform.Find("Player ID/Text").GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        if (player == null) return;

        float percentHealth = ((float)player.stats.health) / player.stats.baseStats.baseHealth;
        healthBar.materialForRendering.SetFloat("_FillAmount", percentHealth);
        // healthBar.fillAmount = percentHealth;

        float percentMana = player.stats.mana / player.stats.baseStats.baseMana;
        manaBar.materialForRendering.SetFloat("_FillAmount", percentMana);
        // manaBar.fillAmount = percentMana;
    }

    public void SetPlayer(PlayerManager playerManager, PlayerSetting playerSetting)
    {
        this.player = playerManager;
        this.playerSetting = playerSetting;

        // annoying workaround because Unity doesn't instance materials for UI images
        healthBar.material = healthBarMaterials[playerSetting.playerIndex];
        manaBar.material = manaBarMaterials[playerSetting.playerIndex];

        playerID.text = "P" + (playerSetting.playerIndex + 1);
        portraitBg.color = playerSetting.team.teamColor + (Color.white * 0.4f);

        ComputePositionAndScale();
    }

    public void ComputePositionAndScale()
    {

        float canvasWidth = transform.root.GetComponent<RectTransform>().rect.width;

        RectTransform rectTrans = GetComponent<RectTransform>();
        rectTrans.anchoredPosition = new Vector2()
        {
            x = (canvasWidth / 4) * playerSetting.playerIndex,
            y = 0
        };

        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvasWidth / 4);
    }

}
