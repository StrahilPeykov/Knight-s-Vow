using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellBallWhileEnraged : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] HealthBar playerHealthbar;
    public int damageOnHit;
    public GameObject impactEffect; 

    //// Start is called before the first frame update
    void Start()
    {
       

       
        player = FindObjectOfType<Player>();
        playerHealthbar = player.healthBar;
        


    }



    ///// <summary>What happens when the ball hits something</summary>
    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //if the ball hits the player, his health points will be reduced
        Player playerr = hitInfo.GetComponent<Player>(); //checks if the collider we hit is that of a player ; using a different variable so as not to override the old one ; the old one is needed to access player's components
        Boss boss = hitInfo.GetComponent<Boss>();
        if (boss != null)  // if we hit the boss, we do nothing and continue the process, we don't want him to take damage from his own attack
        {
            return;

        }
        else if (playerr != null) // if we hit player, we take some of his life
        {
            TakePlayerLife();
        }

        //    //spawns a ball on this location
        Instantiate(impactEffect, transform.position, transform.rotation);  //plays Impact animation on hit
        Destroy(gameObject);
    }

      

  ///// <summary>Reduces player's current life depending on ball's damage</summary>/
        private void TakePlayerLife()
        {
            player.currentHealth -= damageOnHit;
            playerHealthbar.SetHealth(player.currentHealth);
        }
       
   
}
