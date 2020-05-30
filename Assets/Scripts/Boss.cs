using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Boss : MonoBehaviour
{
    private Rigidbody2D bossRB;
    [SerializeField] int pointsForBossDeath;
    [SerializeField] Player player;
    [SerializeField] GameObject firePoint; //point we will shoot the spell ball from
    [SerializeField] LevelExit levelExit;

    private bool isFacingLeft; //used for the Flip method logic
    public HealthBarBoss bossHealthBar;
    [SerializeField] Animator bossAnimator;

    //movement
    [SerializeField] Vector2 moveDirection; //determining whether he is moving right or left (x or -x )
    [SerializeField] float movementSpeed;
    
    //Health
    public int bossMaxHealth = 450;
    public int bossCurrentHealth;
    public bool isAlive ;    //checking if he's alive (used for shooting and moving logic )

    //Enrage
    public bool isHalfHealth;
    public bool isThirdHealth;
    [SerializeField] TextMeshProUGUI text;

    //Start is called before the first frame update
    void Start()
    {
        //making references so that we won't to have to drag them from the inspectir
        player = FindObjectOfType<Player>();
        bossHealthBar = FindObjectOfType<HealthBarBoss>();
        bossRB = GetComponent<Rigidbody2D>();
        firePoint = GameObject.FindGameObjectWithTag("firePoint");
        bossAnimator = this.GetComponent<Animator>();
        levelExit = FindObjectOfType<LevelExit>();
        levelExit.GetComponent<BoxCollider2D>().enabled = false;
        bossCurrentHealth = bossMaxHealth; // current health at the beginning should always be the max health
        bossHealthBar.slider.maxValue = bossMaxHealth; // assigning the Max value of the slider so that it corresponds with the max health of the boss
        bossHealthBar.SetHealth(bossMaxHealth); // setting the value of the Boss's healthbar to the health
        isAlive = true;
        text.text = "Stage 1: Dodge the boss's orbs";
    }

    // Update is called once per frame
    void Update()
    {
        HealthChecks();
        Move();
    }

    ///<summary>Controls the player movement on the x axis(horizontal)</summary>
    public void HandleMovement()
    {
        moveDirection = (player.transform.position - transform.position).normalized * movementSpeed; // determines his movement - we want him to follow the player so it is the player's position minus his own.
        //making him actually follow the player
        bossRB.velocity = new Vector2(moveDirection.x, moveDirection.y);
        Flip();     
    }
  
    ///<summary>flips the gameobject and his children ( fire point ) depending on what direction he is facing</summary>
    private void Flip()
    {
        if (moveDirection.x < 0 && isFacingLeft || moveDirection.x > 0 && !isFacingLeft) // checking what direction he is going and if he is facing the right way
        {
            isFacingLeft = !isFacingLeft;
            //turning the player
            bossAnimator.SetBool("isStandingStill", false);
            bossAnimator.SetBool("isMoving", true);  // triggering the moving animation
            transform.Rotate(0f, 180f, 0f); // rotating him
        }

    }
  
    ///<summary>If he is attacked, this method is called in the PlayerCombat class. His health is reduced with the amount of damage given</summary>
   public void TakeLife(int damage)
    {
        {
            bossCurrentHealth = bossCurrentHealth- damage;
            bossHealthBar.SetHealth(bossCurrentHealth); // setting the healthbar value again
            Die();
        } 
   }

    ///<summary>Dies if his health reaches / goes below zero </summary>
    private void Die()
    {
        if (bossCurrentHealth <= 0)
        {          
            isAlive = false;
            this.GetComponent<PolygonCollider2D>().enabled = false;
            bossAnimator.SetBool("isDead", true); // triggering his animation
            FindObjectOfType<GameSession>().AddToScore(pointsForBossDeath); // adding boss's points to the score
            levelExit.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    ///<summary> The moving logic with its checks </summary> 
    private void Move()
    {
        if (isHalfHealth==false && (!bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("BossIntro") && !bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) )  //if his intro animation is NOT playing and he is alive, he will start moving
        {
            HandleMovement();
        }
    }

    private void HealthChecks()
    {
        if (bossCurrentHealth <= bossMaxHealth / 2)  // premesti go v drugiq metod
        {
            isHalfHealth = true;
            bossAnimator.SetBool("isAttacking", true);
            bossAnimator.SetBool("isMoving", false);
            text.text = "Stage 2: Watch the colored tiles!";
        }

        if (bossCurrentHealth <= bossMaxHealth / 3)
        {
            isHalfHealth = false;
            isThirdHealth = true;
            bossAnimator.SetBool("isAttacking", false);
            bossAnimator.SetBool("isMoving", true);
            text.text = "Stage 3: Dodge the orbs!They do more damage now";
        }     
    }
}

    
   

