using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] Rigidbody2D rb;
    public Player player;
    [SerializeField] PlayerCombat pc;
    [SerializeField] GameObject impactEffect;
    [SerializeField] int damage = 40;
    private float playerDirection;
    private bool facingRight;

    void Start()
    {
        pc = FindObjectOfType<PlayerCombat>();
        player = FindObjectOfType<Player>();
        playerDirection = player.controlThrow;
        rb.velocity = transform.right * speed;
    } 
    
      /// <summary>Gives info about what the fire ball has hit </summary>     
     void OnTriggerEnter2D(Collider2D hitInfo)
    {
        //checking if what we've hit is one of the three
        Player player = hitInfo.GetComponent<Player>();
        Enemy_Rat rat = hitInfo.GetComponent<Enemy_Rat>();
        Boss boss = hitInfo.GetComponent<Boss>();
        if (player != null)
        {
            return; // we don't want the player to takes damage when he hits himself
        }

        //boss/rat take damage if hit
        else if (rat != null)
        {
            rat.TakeDamage(damage); 
        }
        else if (boss!= null)
        {
            boss.TakeLife(damage);
        }
        Debug.Log(hitInfo); // writes the name of what we've hit in the Debug panel
        Instantiate(impactEffect, transform.position, transform.rotation); // spawns the explosion effect on the position it has hit
        Destroy(gameObject);
     }
}
