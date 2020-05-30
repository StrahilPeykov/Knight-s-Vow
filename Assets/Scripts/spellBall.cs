using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spellBall : MonoBehaviour
{
    [SerializeField] Player player;
    public float speed = 20f;
    public Rigidbody2D rb; //RigidBody component of the spell ball
    [SerializeField] HealthBar playerHealthbar;
    public int damageOnHit;
    public GameObject impactEffect; // animation that plays on hit
    Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        //reference
        rb = this.GetComponent<Rigidbody2D>();
        //if we use transform.forward it will move on the Z axis, but it is a 2d game so we use right instead
        player = FindObjectOfType<Player>();
        playerHealthbar = player.healthBar;
        Move();
    }

    /// <summary>What happens when the ball hits something</summary>
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

        //spawns a ball on this location
        Instantiate(impactEffect, transform.position, transform.rotation);  //plays Impact animation on hit
        Debug.Log(hitInfo.name); // tells us what we've hit
        Destroy(gameObject); //destroys the ball on collision

    }

    /// <summary>Reduces player's current life depending on ball's damage</summary>/
    private void TakePlayerLife()
    {
        if (FindObjectOfType<Boss>().GetComponent<Boss>().isThirdHealth)
        {
            player.currentHealth -= (damageOnHit+5);
        }
        else
        {
            player.currentHealth -= damageOnHit;
        }
        playerHealthbar.SetHealth(player.currentHealth);
    }

    /// <summary>Moves ball towards player's current position</summary>
    private void Move()
    {
        moveDirection = (player.transform.position - transform.position).normalized * speed; // // determines movement - we want it to follow the player so it is the player's position minus its own
        rb.velocity = new Vector2(moveDirection.x, moveDirection.y); //making it actually follow the player
    }
}
