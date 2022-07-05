using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerManager player;
    public Gradient healthGradient;
    private Image fill;

    private void Start()
    {
        slider.maxValue = player.stats.baseStats.baseHealth;
        fill = transform.Find("Fill").GetComponent<Image>();
    }

    private void Update()
    {
        UpdateHealthBar();

    }

    public void UpdateHealthBar()
    {
        slider.value = Mathf.Max(0, player.stats.health);
        fill.color = healthGradient.Evaluate(slider.normalizedValue);
    }
}
