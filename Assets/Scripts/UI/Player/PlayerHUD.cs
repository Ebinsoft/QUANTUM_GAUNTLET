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
    Image[] stockIcons;
    Text debugText;

    void Awake()
    {
        healthBar = transform.Find("Health/Bar").GetComponent<Image>();
        manaBar = transform.Find("Mana/Bar").GetComponent<Image>();
        portraitBg = transform.Find("Portrait/BG").GetComponent<Image>();
        playerID = transform.Find("Player ID/Text").GetComponent<TMPro.TextMeshProUGUI>();

        debugText = transform.Find("Debug Text").GetComponent<Text>();
    }

    void Update()
    {
        if (player == null) return;

        float percentHealth = ((float)player.stats.health) / player.stats.baseStats.baseHealth;
        healthBar.materialForRendering.SetFloat("_FillAmount", percentHealth);

        float percentMana = player.stats.mana / player.stats.baseStats.baseMana;
        manaBar.materialForRendering.SetFloat("_FillAmount", percentMana);

        debugText.text = player.currentState.ToString();
    }

    void OnApplicationQuit()
    {
        // reset materials on quit so they stop showing up in the git diff
        healthBar.materialForRendering.SetFloat("_FillAmount", 1);
        manaBar.materialForRendering.SetFloat("_FillAmount", 1);
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

        GenerateStockIcons();

        // subscribe to player deaths
        player.stats.onPlayerDie += onPlayerDie;

        ComputePositionAndScale();
    }

    public void ComputePositionAndScale()
    {
        float canvasWidth = transform.root.GetComponent<RectTransform>().rect.width;
        float padding = canvasWidth / 64;

        RectTransform rectTrans = GetComponent<RectTransform>();
        rectTrans.anchoredPosition = new Vector2()
        {
            x = (canvasWidth / 4) * playerSetting.playerIndex + padding,
            y = 0
        };

        float hudWidth = (canvasWidth / 4) - (2 * padding);
        float hudHeight = (1f / 3) * hudWidth;

        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hudWidth);
        rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, hudHeight);
    }

    private void GenerateStockIcons()
    {
        GameObject stocksParent = transform.Find("Stock Icons").gameObject;

        int numLives = GameManager.instance.versusInfo.numLives;
        stockIcons = new Image[numLives];
        stockIcons[0] = stocksParent.transform.Find("Stock 1").GetComponent<Image>();
        Vector2 originalPosition = stockIcons[0].gameObject.GetComponent<RectTransform>().anchoredPosition;

        for (int i = 1; i < numLives; i++)
        {
            GameObject stock = (GameObject)Instantiate(stockIcons[0].gameObject);
            stock.transform.SetParent(stocksParent.transform);
            RectTransform rectTrans = stock.GetComponent<RectTransform>();
            rectTrans.anchoredPosition = originalPosition + new Vector2() { x = 18 * i, y = 0 };

            stockIcons[i] = stock.GetComponent<Image>();
        }
    }

    private void onPlayerDie(GameObject playerObj)
    {
        // update stock icons
        int remainingLives = player.stats.lives;

        for (int i = 0; i < stockIcons.Length; i++)
        {
            if (i >= remainingLives)
            {
                stockIcons[i].color = Color.gray;
            }
        }
    }
}
