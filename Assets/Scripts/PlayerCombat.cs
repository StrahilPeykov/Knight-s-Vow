using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerCombat : MonoBehaviour
{
    #region props
    [SerializeField] Animator animator;
    public GameObject attackPoint;
    [SerializeField] Player player;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayer;


    public bool canDashing = true;
    [SerializeField] float dashingDuration = 1f;
    [SerializeField] GameObject fireBall;

    [SerializeField] Rigidbody2D myRigidBody;
    public int attackDamage = 5;
    public float attackRate = 1f;
    float nextAttackTime = 0f;
    #endregion 

    private void Start()
    {
        //making references
        player = this.GetComponent<Player>();
        attackPoint = GameObject.FindGameObjectWithTag("attackPoint");
        myRigidBody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Dash();
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Strike();
                nextAttackTime = Time.time + 2f / attackRate;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                FireBall();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    /// <summary>Main attack</summary>
    void Attack()
    {
        animator.SetTrigger("Attack");   //plays animation
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Boss>() == null) // if the collider is not that of the boss, then rat takes dmg
            {
                enemy.GetComponent<Enemy_Rat>().TakeDamage(attackDamage);
            }
            else
            {
                enemy.GetComponent<Boss>().TakeLife(attackDamage);
            }
        }
    }

    void Dash()
    {
        if (canDashing)
        {
            animator.SetTrigger("Dashing");
            int manaCost = 30;
            player.SpendMana(manaCost);
            StartCoroutine(Dashing(dashingDuration));
        }
    }

    IEnumerator Dashing(float dashingDuration)
    {
        canDashing = false;
        float controlThrow = Input.GetAxis("Horizontal");
        float time = 0;
        while (dashingDuration > time)
        {
            time += Time.deltaTime;
            myRigidBody.velocity = new Vector2(controlThrow * 7f, myRigidBody.velocity.y);
            yield return 0;
        }
        canDashing = true;
        yield return new WaitForSeconds(dashingDuration);
    }

    void Strike()
    {
        int manaCost = 50;
        if (manaCost <= player.currentMana)
        {
            animator.SetTrigger("Strike");
            player.SpendMana(manaCost);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.transform.position, attackRange, enemyLayer);
            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.GetComponent<Boss>() == null) // if the collider is not that of the boss, then rat takes dmg
                {
                    enemy.GetComponent<Enemy_Rat>().TakeDamage(attackDamage * 3);
                }
                else
                {
                    enemy.GetComponent<Boss>().TakeLife(attackDamage * 3);
                }
            }
        }
    }

    /// <summary>Casts a fireball </summary>
    void FireBall()
    {
        int manaCost = 100;
        if (manaCost <= player.currentMana) // check if we have enough mana
        {
            animator.SetTrigger("CastingSpell"); //playing animation
            player.SpendMana(manaCost);
            Instantiate(fireBall, attackPoint.transform.position, attackPoint.transform.rotation); // spawning fireball on attack point's position
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRange);
    }
}