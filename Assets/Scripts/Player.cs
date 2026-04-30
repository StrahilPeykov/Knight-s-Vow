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
    bool isDamageImmune = false;

    // Cached component references
    Rigidbody2D myRigidBody;
    public Animator myAnimator;
    CapsuleCollider2D myBodyCollider2D;
    BoxCollider2D myFeet;
    float gravityScaleAtStart;
    readonly Collider2D[] touchingColliders = new Collider2D[16];

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
        gs = FindObjectOfType<GameSession>();
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
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.linearVelocity.y);
        myRigidBody.linearVelocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.linearVelocity.x) > Mathf.Epsilon;
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
        Vector2 climbVelocity = new Vector2(myRigidBody.linearVelocity.x, controlThrow * climbSpeed);
        myRigidBody.linearVelocity = climbVelocity;
        myRigidBody.gravityScale = 0f;

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidBody.linearVelocity.y) > Mathf.Epsilon;
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
            myRigidBody.linearVelocity += jumpVelocityToAdd;
        }
    }

    /// <summary>Reduces player's life if he touches enemy</summary>
    public void TakeLife()
    {
        if (isDamageImmune || !myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            return;
        }

        int touchingCount = myBodyCollider2D.GetContacts(touchingColliders);
        for (int i = 0; i < touchingCount; i++)
        {
            Enemy_Rat enemy = touchingColliders[i].GetComponent<Enemy_Rat>();
            if (enemy != null && enemy.enabled)
            {
                StartCoroutine(ImmuneToDamage());
                myAnimator.SetTrigger("GettingHurt"); // playing animation
                myRigidBody.linearVelocity = deathKick;
                currentHealth -= enemy.attackDamage;
                if (healthBar != null)
                {
                    healthBar.SetHealth(currentHealth);
                }
                break;
            }
        }
    }

    /// <summary>Dies if health reaches/goes below zero or if he touches spikes</summary>
    public void Die()
    {
        bool touchedHazard = myBodyCollider2D.IsTouchingLayers(LayerMask.GetMask("Hazards")) || myFeet.IsTouchingLayers(LayerMask.GetMask("Hazards"));
        if (!touchedHazard && currentHealth > 0)
        {
            return;
        }

        isAlive = false;
        myAnimator.SetTrigger("Dying"); // playing animation
        myRigidBody.linearVelocity = deathKick;
        if (gs == null)
        {
            gs = FindObjectOfType<GameSession>();
        }
        if (gs != null)
        {
            gs.ProcessPlayerDeath();
        }
    }

    /// <summary> Making the player immune to damage for x seconds and disabling collision with enemies</summary>
    IEnumerator ImmuneToDamage()
    {
        isDamageImmune = true;
        myBodyCollider2D.enabled = false;
        yield return new WaitForSeconds(immuneToDamageDelay);
        myBodyCollider2D.enabled = true;
        isDamageImmune = false;
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
        if (gs == null)
        {
            gs = FindObjectOfType<GameSession>();
        }
        SaveSystem.SavePlayer(this, gs);

    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            Debug.LogWarning("No player save file found.");
            return;
        }
        maxHealth = data.maxHealth;
        currentHealth = data.currentHealth;
        maxMana = data.maxMana;
        currentMana = data.currentMana;
        gs = FindObjectOfType<GameSession>();
        if (gs != null)
        {
            gs.SetScore(data.score);
        }
        // gs.sceneIndex = data.sceneIndex;
        if (manaBar != null)
        {
            manaBar.SetMana((int)currentMana);
        }
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }
}
