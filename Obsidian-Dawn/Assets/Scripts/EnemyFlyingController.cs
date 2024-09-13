using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    public float RangeToStartChase;  // The distance at which the enemy starts chasing the player
    public float MoveSpeed;          // The speed at which the enemy moves towards the player
    public float TurnSpeed;          // How quickly the enemy turns to face the player

    public Animator EnemyFlyingAnimator;  // Animator used to control enemy's animation states

    private bool isChasing;          // Tracks whether the enemy is currently chasing the player
    private Transform player;        // Reference to the player character

    // Initialize and find the player when the game starts
    void Start()
    {
        // The enemy references the player's transform, assuming the player health controller is always active
        player = PlayerHealthController.Instance.transform;
    }

    // Main logic for enemy behavior, executed every frame
    void Update()
    {
        // If the enemy is not yet chasing the player, check the distance to the player
        if (!isChasing)
        {
            // Start chasing if the player is within range
            if (Vector3.Distance(transform.position, player.position) < RangeToStartChase)
            {
                isChasing = true;
                EnemyFlyingAnimator.SetBool("isChasing", isChasing);  // Update the enemy's animation state
            }
        }
        else
        {
            // Continue chasing the player if the player is still active
            if (player.gameObject.activeSelf)
            {
                // Calculate the direction to the player and rotate the enemy to face them
                Vector3 direction = transform.position - player.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Smoothly rotate the enemy towards the player over time
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);

                // Move the enemy towards the player
                transform.position += -transform.right * MoveSpeed * Time.deltaTime;
            }
        }
    }
}
