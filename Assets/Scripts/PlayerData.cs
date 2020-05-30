using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public float currentMana;
    public int score;
    // int sceneIndex;

    public PlayerData(Player player, GameSession gs)
    {
        maxHealth = player.maxHealth;
        currentHealth = player.currentHealth;
        maxMana = player.maxMana;
        currentMana = player.currentMana;
        gs = GameObject.FindObjectOfType<GameSession>();
        score = gs.score;
        //sceneIndex = gs.sceneIndex;
    }
}
