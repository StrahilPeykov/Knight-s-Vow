using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPickup : MonoBehaviour
{
    [SerializeField] AudioClip gemPickUpSFX;  // Sound that plays when Gem is picked up
    [SerializeField] int pointsForGemPickup = 100; // points player gets when Gem is picked up
    private Player player;

    //activates if someone touches the collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player>();
        if (player == null)
        {

        }
        else
        {
            FindObjectOfType<GameSession>().AddToScore(pointsForGemPickup); // adding points to the score
            AudioSource.PlayClipAtPoint(gemPickUpSFX, Camera.main.transform.position); // playes the sound effect
            Destroy(gameObject);
        }
    }
}
