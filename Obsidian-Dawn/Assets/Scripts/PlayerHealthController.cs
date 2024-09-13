using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of PlayerHealthController exists across the game
    public static PlayerHealthController Instance;

    [HideInInspector]
    public int CurrentHealth; // The player's current health value, hidden in the Inspector to prevent accidental modification
    public int MaxHealth; // The player's maximum health value

    public float InvincibilityLength; // Duration of invincibility after taking damage
    public float FlashLength; // Duration between flashes during invincibility, for visual feedback

    public SpriteRenderer[] PlayerSprites; // Array of player sprites to enable/disable for flashing effect

    private float invincibilityCounter; // Tracks the remaining invincibility time
    private float flashCounter; // Tracks the timing between flashes

    // Ensures only one instance of PlayerHealthController exists and persists across scenes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the PlayerHealthController active across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates if one already exists
        }
    }

    // Initialize player's health to the maximum at the start of the game
    void Start()
    {
        CurrentHealth = MaxHealth;
    }

    // Handles invincibility and flashing after the player takes damage
    void Update()
    {
        // If the player is currently invincible, handle flashing and countdown
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
            flashCounter -= Time.deltaTime;

            // Toggle sprite visibility to create a flashing effect while invincible
            if (flashCounter <= 0)
            {
                foreach (SpriteRenderer spriteRenderer in PlayerSprites)
                {
                    spriteRenderer.enabled = !spriteRenderer.enabled;
                }
                flashCounter = FlashLength; // Reset flash timer
            }

            // Once invincibility ends, make sure sprites are fully visible
            if (invincibilityCounter <= 0)
            {
                foreach (SpriteRenderer spriteRenderer in PlayerSprites)
                {
                    spriteRenderer.enabled = true; // Ensure player sprite is visible
                }
                flashCounter = 0f; // Reset flash counter
            }
        }
    }

    // Reduces the player's health by the specified amount, handles death and invincibility
    public void DamagePlayer(int damageAmount)
    {
        // Only apply damage if the player isn't invincible
        if (invincibilityCounter <= 0)
        {
            CurrentHealth -= Mathf.Abs(damageAmount); // Subtract damage from current health

            // Handle player death and respawn if health reaches zero
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0; // Clamp health to zero
                RespawnController.Instance.Respawn(); // Trigger respawn sequence
                AudioManager.Instance.PlaySFX(8); // Play death sound effect
            }
            else
            {
                // Trigger invincibility after taking damage
                invincibilityCounter = InvincibilityLength;
                AudioManager.Instance.PlaySFXAdjusted(11); // Play damage sound effect
            }

            // Update the health UI to reflect the current health
            UIController.Instance.UpdateHealth(CurrentHealth, MaxHealth);
        }
    }

    // Fully restores the player's health to maximum
    public void FillHealth()
    {
        CurrentHealth = MaxHealth;
        UIController.Instance.UpdateHealth(CurrentHealth, MaxHealth); // Update the health UI
    }

    // Heals the player by the specified amount, ensuring health doesn't exceed max
    public void HealPlayer(int healAmount)
    {
        CurrentHealth += Mathf.Abs(healAmount); // Add healing value to current health

        // Ensure health doesn't exceed the maximum limit
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }

        // Update the health UI to reflect the new health value
        UIController.Instance.UpdateHealth(CurrentHealth, MaxHealth);
    }
}
