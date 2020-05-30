using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ManaBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Text manaText;

    /// <summary>Setting the mana to max </summary>
    public void SetMaxMana(int mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
        manaText.text = slider.value + "/" + slider.value;
    }

    /// <summary>Setting he mana </summary>
    public void SetMana(int mana)
    {
        slider.value = mana;
        manaText.text = slider.value + "/" + slider.maxValue;
    }
}
