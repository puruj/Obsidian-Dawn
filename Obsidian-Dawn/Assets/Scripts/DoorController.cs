using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public Animator DoorAnimator;

    [SerializeField]
    private float distanceToOpen;

    private PlayerController playerController;

    private bool playerExiting;

    [SerializeField]
    private Transform exitPoint;
    [SerializeField]
    private float movePlayerSpeed;

    [SerializeField]
    private string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        playerController = PlayerHealthController.Instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, playerController.transform.position) < distanceToOpen)
        {
            DoorAnimator.SetBool("doorOpen", true);
        }
        else
        {
            DoorAnimator.SetBool("doorOpen", false);
        }

        if (playerExiting)
        {
            playerController.transform.position = Vector3.MoveTowards(playerController.transform.position, exitPoint.position, movePlayerSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!playerExiting)
            {
                playerController.CanMove = false;
                StartCoroutine(CoroutineUseDoor());
            }
        }
    }

    private IEnumerator CoroutineUseDoor()
    {
        playerExiting = true;
        playerController.PlayerAnimator.enabled = false;

        UIController.Instance.StartFadeToBlack();

        yield return new WaitForSeconds(1.5f);

        RespawnController.Instance.SetSpawn(exitPoint.position);
        playerController.CanMove = true;
        playerController.PlayerAnimator.enabled = true;

        UIController.Instance.StartFadeFromBlack();


        PlayerPrefs.SetString("ContinueLevel", levelToLoad);
        PlayerPrefs.SetFloat("PosX", exitPoint.position.x);
        PlayerPrefs.SetFloat("PosY", exitPoint.position.y);
        PlayerPrefs.SetFloat("PosZ", exitPoint.position.z);
        

        SceneManager.LoadScene(levelToLoad);
    }
}
