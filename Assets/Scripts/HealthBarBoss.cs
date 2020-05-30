using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBoss : MonoBehaviour
{
    public Slider slider;
    [SerializeField] Text healthText;

    /// <summary>Sets health to max </summary>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;  // setting the bar's value to represent Boss's health
        healthText.text = slider.value + "/" + slider.maxValue; //Text that is written on the bar
    }

    /// <summary>Sets the health (mostly used for setting current health) </summary>
    public void SetHealth(int health)
    {
        slider.value = health;
        healthText.text = slider.value + "/" + slider.maxValue;
    }
}
