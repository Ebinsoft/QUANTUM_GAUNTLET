using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Slider slider;
    public PlayerManager player;
    public Gradient manaGradient;
    private Image fill;

    private void Start()
    {
        slider.maxValue = player.stats.baseStats.baseMana;
        fill = transform.Find("Fill").GetComponent<Image>();
    }

    private void Update()
    {
        UpdateManaBar();

    }

    public void UpdateManaBar()
    {
        slider.value = Mathf.Max(0, player.stats.mana);
        fill.color = manaGradient.Evaluate(slider.normalizedValue);
    }
}
