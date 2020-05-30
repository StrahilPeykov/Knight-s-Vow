using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPickUp : MonoBehaviour
{
    [SerializeField] AudioClip potionPickUpSFX;  // sound effect on pickup
    [SerializeField] int healthRegenPoints = 30;
    [SerializeField] Player player;
    
    private void Start()
    {
        //reference to player
        player = FindObjectOfType<Player>();
    }

    /// <summary>Activates when it touches someone's collider </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player>();

        //if someone that is not the player took the potion, do nothing
        if (player == null)
        {

        }
        else
        {
            //if player's health + potion HP regeneration points exceeds his max health, his currenthealth will reach max instead
            if (player.currentHealth + healthRegenPoints > player.maxHealth)
            {
                player.currentHealth = player.maxHealth;
            }
            else
            {
                player.currentHealth += healthRegenPoints;

            }
            player.healthBar.SetHealth(player.currentHealth); //setting health
            AudioSource.PlayClipAtPoint(potionPickUpSFX, Camera.main.transform.position); //playing audio
            Destroy(gameObject);        
        }    
    }
}
