using UnityEngine;

public class Grappler : MonoBehaviour
{
    public Transform player;
    private bool hasInteracted = false;  // Flag to check if interaction has occurred

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if already interacted or if the colliding object is not an enemy
        if (hasInteracted || !collision.CompareTag("Enemy"))
        {
            return;
        }

        // Set the flag to true to prevent further interactions
        hasInteracted = true;

        // Call the move to enemy function on the player
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            Debug.Log("voy al enemmigo");
            playerController.MoveToEnemy(collision.transform.position);
        }

        // Optionally, destroy the enemy here or do other interactions
        Destroy(collision.gameObject);
    }
}
