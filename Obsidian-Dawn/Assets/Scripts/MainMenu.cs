using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] 
    private string newGameScene;

    [SerializeField]
    private GameObject continueButton;

    [SerializeField]
    private PlayerAbilityTracker playerAbilityTracker;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("ContinueLevel"))
        {
            continueButton.SetActive(true);
        }
        AudioManager.Instance.PlayMainMenuMusic();
    }
    
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(newGameScene);
    }

    public void Continue()
    {
        playerAbilityTracker.gameObject.SetActive(true);
        playerAbilityTracker.transform.position = new Vector3(PlayerPrefs.GetFloat("PosX"), PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));

        if (PlayerPrefs.HasKey("DoubleJumpUnlocked"))
        {
            if (PlayerPrefs.GetInt("DoubleJumpUnlocked") == 1)
            {
                playerAbilityTracker.CanDoubleJump = true;
            }
        }

        if (PlayerPrefs.HasKey("DashUnlocked"))
        {
            if (PlayerPrefs.GetInt("DashUnlocked") == 1)
            {
                playerAbilityTracker.CanDash = true;
            }
        }

        if (PlayerPrefs.HasKey("BallUnlocked"))
        {
            if (PlayerPrefs.GetInt("BallUnlocked") == 1)
            {
                playerAbilityTracker.CanBecomeBall = true;
            }
        }

        if (PlayerPrefs.HasKey("BombUnlocked"))
        {
            if (PlayerPrefs.GetInt("BombUnlocked") == 1)
            {
                playerAbilityTracker.CanDropBomb = true;
            }
        }

        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
