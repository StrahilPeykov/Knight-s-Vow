using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Config
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] float immuneToDamageDelay = 3f;
    [SerializeField] private GameSession gs;

    private Enemy_Rat enemy_rat;
    public float controlThrow;

    // Player Health
    public int maxHealth = 150;
    public int currentHealth;
    public HealthBar healthBar;
    public bool facingLeft = false;

    //Player mana

    public int maxMana = 100;
    public float currentMana;
    public int manaRegen = 13;
    public ManaBar manaBar;

    // State
    bool isAlive = true;

    // Cached component references
    Rigidbody2D myRigidBody;
    public Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = FindObjectOfType<HealthBar>();
        manaBar = FindObjectOfType<ManaBar>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        currentMana = maxMana;
        manaBar.SetMaxMana(maxMana);


        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider2D = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidBody.gravityScale;
        enemy_rat = FindObjectOfType<Enemy_Rat>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        ManaRegen();
        Run();
        ClimbLadder();
        Jump();
        TakeLife();
        Flip();
        Die();
    }

    /// <summary>Runs when there is input</summary>
    private void Run()
    {
        controlThrow = Input.GetAxis("Horizontal"); // Value is between -1 and +1
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("Running", playerHasHorizontalSpeed); // plays running animation
    }

    /// <summary>Climbs on the ladder if on its layer</summary>
    private void ClimbLadder()
    {
        if (!myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"))) // checking if player is touching the ladder
        {
            myAnimator.SetBool("Climbing", false);
            myRigidBody.gravityScale = gravityScaleAtStart;
            return;
        }
        float controlThrow = Input.GetAxis("Vertical"); // changind moving direction to vertical
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
        myRigidBody.velocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("Climbing", playerHasVerticalSpeed);
    }

    /// <summary>Jumps if on ground/boss</summary>
    private void Jump()
    {
        if (!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myFeet.IsTouchingLayers(LayerMask.GetMask("Boss")) && !myFeet.IsTouchingLayers(LayerMask.GetMask("Enemy"))) // we want him to be able to jump on Boss's head
        {
            return;
        }
        if (Input.GetButtonDown("Jump")) // jumps if jump button is pressed
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    /// <summary>Reduces player's life if he touches enemy</summary>
    public void TakeLife()
    {
        if (enemy_rat is null)
        {
            // if there is no rat on the level, do nothing
        }
        else
        {
            int enemyDamage = enemy_rat.attackDamage;
            if ((myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")) && enemy_rat.enabled == true)) // checking if he is touching enemy
            {
                StartCoroutine(ImmuneToDamage());
                myAnimator.SetTrigger("GettingHurt"); // playing animation
                myRigidBody.velocity = deathKick;
                currentHealth -= enemyDamage;
                healthBar.SetHealth(currentHealth);
            }
        }
    }

    /// <summary>Dies if health reaches/goes below zero or if he touches spikes</summary>
    public void Die()
    {
        if (myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Hazards"))) // checking if he is touching spikes
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying"); // playing animation
            myRigidBody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
        if (currentHealth <= 0)
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidBody.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    /// <summary> Making the player immune to damage for x seconds and disabling collision with enemies</summary>
    IEnumerator ImmuneToDamage()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
        yield return new WaitForSeconds(immuneToDamageDelay);
        GetComponent<CapsuleCollider2D>().enabled = true;
    }

    ///<summary>flips the gameobject and his children ( attack point ) depending on what direction he is facing</summary>
    private void Flip()
    {
        if (controlThrow > 0 && facingLeft || controlThrow < 0 && !facingLeft)
        {
            facingLeft = !facingLeft;
            //turning the player
            transform.Rotate(0f, 180f, 0f);
        }
    }

    /// <summary>Spends Mana </summary>
    public void SpendMana(int mana)
    {
        //checks if player has enough mana
        if (mana <= currentMana)
        {
            currentMana -= mana;
            manaBar.SetMana((int)currentMana);

        }
        else if (mana > currentMana)
        {

            Debug.Log("No mana!");
        }
    }

    /// <summary>Regenerates x mana per second </summary>
    public void ManaRegen()
    {
        //if we have less mana than the max, mana will regenerate per second
        if (currentMana < maxMana)
        {
            currentMana += manaRegen * Time.deltaTime;
            if (currentMana > maxMana)
            {
                int rest = (int)currentMana - maxMana;
                currentMana -= rest;

            }
            manaBar.SetMana((int)currentMana);
        }
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, gs);

    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        maxHealth = data.maxHealth;
        currentHealth = data.currentHealth;
        maxMana = data.maxMana;
        currentMana = data.currentMana;
        gs = FindObjectOfType<GameSession>();
        gs.score = data.score;
        // gs.sceneIndex = data.sceneIndex;
        manaBar.SetMana((int)currentMana);
        healthBar.SetHealth(currentHealth);
    }
}
