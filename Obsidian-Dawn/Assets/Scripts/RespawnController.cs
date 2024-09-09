using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnController : MonoBehaviour
{
    public static RespawnController Instance;

    [SerializeField]
    private float waitToRespawn;

    private Vector3 respawnPoint;

    private GameObject player;

    [SerializeField]
    private GameObject deathEffect;

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
        player = PlayerHealthController.Instance.gameObject;

        respawnPoint = player.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpawn(Vector3 newPosition)
    {
        respawnPoint = newPosition;
    }

    public void Respawn()
    {
        StartCoroutine(CoroutineRespawn());
    }

    IEnumerator CoroutineRespawn()
    {
        player.SetActive(false);

        if (deathEffect != null)
        {
            Instantiate(deathEffect, player.transform.position, player.transform.rotation);
        }

        yield return new WaitForSeconds(waitToRespawn);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        player.transform.position = respawnPoint;
        player.SetActive(true);

        PlayerHealthController.Instance.FillHealth();
    }

}
