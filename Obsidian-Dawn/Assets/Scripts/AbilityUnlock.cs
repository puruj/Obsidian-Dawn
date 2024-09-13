using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    [SerializeField]
    private bool unlockDoubleJump;   // Flags indicating which ability this object unlocks
    [SerializeField]
    private bool unlockDash;
    [SerializeField]
    private bool unlockBecomeBall;
    [SerializeField]
    private bool unlockDropBomb;

    [SerializeField]
    private GameObject pickUpEffect; // Effect to trigger when the ability is unlocked

    [SerializeField]
    private TMP_Text unlockText;     // UI element to display the unlock message
    [SerializeField]
    private string unlockMessage;    // The message shown when an ability is unlocked

    // This method is called when the player enters the trigger zone of the unlock item.
    // It checks the player's abilities and unlocks them accordingly, while also providing
    // visual and audio feedback to the player.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Ensure the object is the player and proceed to unlock abilities
        if (collision.tag == "Player")
        {
            // Access the player's ability tracker to update unlocked abilities
            PlayerAbilityTracker playerAbilityTracker = collision.GetComponentInParent<PlayerAbilityTracker>();

            if (playerAbilityTracker != null)
            {
                // Each of these blocks handles unlocking a specific ability and stores the unlock state
                if (unlockDoubleJump)
                {
                    playerAbilityTracker.CanDoubleJump = true;
                    PlayerPrefs.SetInt("DoubleJumpUnlocked", 1); // Persisting unlock state
                }

                if (unlockDash)
                {
                    playerAbilityTracker.CanDash = true;
                    PlayerPrefs.SetInt("DashUnlocked", 1);
                }

                if (unlockBecomeBall)
                {
                    playerAbilityTracker.CanBecomeBall = true;
                    PlayerPrefs.SetInt("BallUnlocked", 1);
                }

                if (unlockDropBomb)
                {
                    playerAbilityTracker.CanDropBomb = true;
                    PlayerPrefs.SetInt("BombUnlocked", 1);
                }
            }

            // Trigger visual and audio feedback to signify the ability has been unlocked
            Instantiate(pickUpEffect, transform.position, Quaternion.identity);

            // Display the unlock message to the player, temporarily detached from the world
            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position;
            unlockText.text = unlockMessage;
            unlockText.gameObject.SetActive(true);

            // Clean up the unlock message and the unlock object after a brief delay
            Destroy(unlockText.transform.parent.gameObject, 5f);
            Destroy(gameObject);

            // Play the sound effect associated with the ability unlock
            AudioManager.Instance.PlaySFX(5);
        }
    }
}
