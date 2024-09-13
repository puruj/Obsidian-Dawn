using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Slider HealthSlider;

    [SerializeField]
    private Image fadeScreen;
    [SerializeField]
    private float fadeSpeed = 2f;
    [SerializeField]
    private bool fadingToBlack;
    [SerializeField]
    private bool fadingfromBlack;

    [SerializeField]
    private string mainMenuScene;

    [SerializeField]
    private GameObject pauseScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
            {
                fadingToBlack = false;
            }
        }
        else if (fadingfromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                fadingfromBlack = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnPause();
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        if (HealthSlider != null)
        {
            HealthSlider.maxValue = maxHealth;
            HealthSlider.value = currentHealth;
        }
    }

    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingfromBlack = false;
    }

    public void StartFadeFromBlack()
    {
        fadingfromBlack = true;
        fadingToBlack = false;
    }

    public void PauseUnPause()
    {
        if (!pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;

        Destroy(PlayerHealthController.Instance.gameObject);
        PlayerHealthController.Instance = null;

        Destroy(RespawnController.Instance.gameObject);
        RespawnController.Instance = null;

        Instance = null;
        Destroy(gameObject);

        SceneManager.LoadScene(mainMenuScene);
    }
}
