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
        score = gs != null ? gs.score : 0;
        //sceneIndex = gs.sceneIndex;
    }
}
