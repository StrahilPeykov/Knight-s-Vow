using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Rat : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] int maxHealth = 20;
    [SerializeField] int currentHealth;
    public int attackDamage = 10;
    [SerializeField] int pointsOnEnemyDeath;

    private void Start()
    {
        //making references and assigning health
        animator = this.GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    /// <summary> This method is called in the PlayerCombat class if the enemy is hit.</summary>
   public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        Die();
    }

    /// <summary> Called when the enemy dies</summary>
    public void Die()
    {
        if (currentHealth <=0)
        {
            FindObjectOfType<GameSession>().AddToScore(pointsOnEnemyDeath); //adding points to the score
            Debug.Log("Enemy Died");
            Destroy(gameObject);
        }
    }
}
