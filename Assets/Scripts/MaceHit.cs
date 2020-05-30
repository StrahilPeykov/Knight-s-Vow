using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaceHit : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] int DamageOnHit;
    [SerializeField] PolygonCollider2D MaceCollider;
    [SerializeField] CapsuleCollider2D playerBody;
    private Animator playerrAnimator;
    [SerializeField] AudioClip maceSound;

    // Start is called before the first frame update
    void Start()
    {
        //making references
        player = FindObjectOfType<Player>();
        MaceCollider = this.GetComponentInChildren<PolygonCollider2D>();
        playerBody = player.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        OnMaceHit();
    }

    /// <summary>Checks if mace hit the player</summary>
    public void OnMaceHit()
    {
        if (MaceCollider.IsTouching(playerBody))
        {
            //making him immune for x second
            player.myAnimator.SetTrigger("GettingHurt");
            StartCoroutine(ImmuneToDamage());
            player.currentHealth = player.currentHealth - DamageOnHit; // reducing health
            player.healthBar.SetHealth(player.currentHealth);
            AudioSource.PlayClipAtPoint(maceSound, Camera.main.transform.position); // playing audio
            if (player.currentHealth <= 0)
            {
                player.Die();
            }
        }
    }

    IEnumerator ImmuneToDamage()
    {
        playerBody.enabled = false;
        yield return new WaitForSeconds(2f);
        playerBody.enabled = true;
    }
}
