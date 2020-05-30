using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text healthText;

    /// <summary>Sets health to max </summary>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;  // setting the bar's value to represent player's health
        healthText.text = slider.value + "/" + slider.maxValue;
    }

    /// <summary>Sets the health (mostly used for setting current health) </summary>
    public void SetHealth(int health)
    {
        slider.value = health;
        healthText.text = slider.value + "/" + slider.maxValue;
    }

    /// <summary>Called in the PotionPickUp class ; adds health from potion </summary>
    public void AddHealthFromPotion(int healthFromPotion)
    {
        slider.value += healthFromPotion;
        healthText.text = slider.value + "/" + slider.maxValue;
    }
}
