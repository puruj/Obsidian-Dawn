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
        SceneManager.LoadScene(PlayerPrefs.GetString("ContinueLevel"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
