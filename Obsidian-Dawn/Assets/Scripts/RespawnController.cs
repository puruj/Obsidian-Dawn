using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    // Singleton pattern to ensure only one RespawnController exists across scenes
    public static RespawnController Instance;

    [SerializeField]
    private float waitToRespawn; // Time delay before respawn occurs

    private Vector3 respawnPoint; // Stores the position where the player will respawn

    private GameObject player; // Reference to the player object

    [SerializeField]
    private GameObject deathEffect; // Visual effect to show when the player dies

    // Ensures only one instance of RespawnController exists and persists across scene changes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the RespawnController active across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicates
        }
    }

    // Initialize references and set the initial respawn point
    void Start()
    {
        // Fetch the player from the PlayerHealthController, assuming it's always active
        player = PlayerHealthController.Instance.gameObject;

        // Set the initial respawn point to the player's starting position in the scene
        respawnPoint = player.transform.position;
    }

    // This method allows updating the player's respawn point to a new position (e.g., after passing a checkpoint)
    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    // Public method to trigger the respawn process
    public void Respawn()
    {
        StartCoroutine(CoroutineRespawn()); // Starts the respawn sequence as a coroutine
    }

    // Coroutine to handle the respawn sequence, including a delay, visual effect, and reloading the scene
    private IEnumerator CoroutineRespawn()
    {
        // Temporarily disable the player when they die
        player.SetActive(false);

        // Spawn a visual effect (death animation, particle effect, etc.) if provided
        if (deathEffect != null)
        {
            Instantiate(deathEffect, player.transform.position, player.transform.rotation);
        }

        // Wait for the specified respawn time before bringing the player back
        yield return new WaitForSeconds(waitToRespawn);

        // Reload the current scene, effectively resetting the environment
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Restore the player's position to the saved respawn point
        player.transform.position = respawnPoint;

        // Reactivate the player after respawning
        player.SetActive(true);

        // Refill the player's health to full
        PlayerHealthController.Instance.FillHealth();
    }
}
