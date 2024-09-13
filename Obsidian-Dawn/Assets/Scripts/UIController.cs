using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // Singleton pattern to ensure only one instance of UIController exists across scenes
    public static UIController Instance;

    // Health bar slider UI component
    public Slider HealthSlider;

    [SerializeField]
    private Image fadeScreen; // Image used to fade the screen in/out
    [SerializeField]
    private float fadeSpeed = 2f; // Speed at which the screen fades
    [SerializeField]
    private bool fadingToBlack; // Control flag for fading to black
    [SerializeField]
    private bool fadingfromBlack; // Control flag for fading from black

    [SerializeField]
    private string mainMenuScene; // Name of the main menu scene

    [SerializeField]
    private GameObject pauseScreen; // Reference to the pause screen UI

    // Ensures only one instance of UIController exists and persists across scene changes
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the UIController alive across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances if one already exists
        }
    }

    // Manages screen fading logic during transitions
    void Update()
    {
        // Handles the transition effect of fading to black
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
            {
                fadingToBlack = false; // Stop fading once screen is fully black
            }
        }
        // Handles the transition effect of fading from black
        else if (fadingfromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                fadingfromBlack = false; // Stop fading once the screen is fully transparent
            }
        }

        // Toggle pause screen when the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }
    }

    // Updates the player's health bar in the UI
    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = maxHealth; // Set max health value in the slider
            HealthSlider.value = currentHealth; // Update the slider with current health
        }
    }

    // Starts the fade-to-black transition, typically used when switching scenes or restarting
    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingfromBlack = false;
    }

    // Starts the fade-from-black transition, commonly used when a new scene loads
    public void StartFadeFromBlack()
    {
        fadingfromBlack = true;
        fadingToBlack = false;
    }

    // Toggles the pause screen on or off, also controls game time (freezes/unfreezes)
    public void PauseUnPause()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f; // Freeze game time when paused
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f; // Resume game time when unpaused
        }
    }

    // Returns the player to the main menu, resetting key components and stopping the game
    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Ensure game time is normal when returning to the main menu

        // Cleanup: destroy key player-related components to ensure no cross-scene dependencies
        Destroy(PlayerHealthController.Instance.gameObject);
        PlayerHealthController.Instance = null;

        Destroy(RespawnController.Instance.gameObject);
        RespawnController.Instance = null;

        // Cleanup the UIController itself as we're transitioning to the main menu
        Instance = null;
        Destroy(gameObject);

        // Load the main menu scene
        SceneManager.LoadScene(mainMenuScene);
    }
}
