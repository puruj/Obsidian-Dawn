using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{

    private CameraController cameraController;
    [SerializeField]
    private Transform camPosition;
    [SerializeField]
    private float camSpeed;

    [SerializeField]
    private int threshold1;
    [SerializeField]
    private float threshold2;

    [SerializeField]
    private float activeTime;
    [SerializeField]
    private float inactiveTime;
    [SerializeField]
    private float fadeOutTime;
    private float activeCounter;
    private float inactiveCounter;
    private float fadeOutCounter;

    [SerializeField]
    private Transform[] spawnPoints;
    private Transform targetPoint;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform boss;

    [SerializeField]
    private float timeBetweenShots1;
    [SerializeField]
    private float timeBetweenShots2;
    private float shotCounter;

    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform shotPoint;


    [SerializeField]
    private GameObject winObjects;

    private bool battleEnded;

    // Start is called before the first frame update
    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
        cameraController.enabled = false;

        activeCounter = activeTime;

        shotCounter = timeBetweenShots1;

        AudioManager.Instance.PlayBossMusic();
    }

    // Update is called once per frame
    void Update()
    {
        cameraController.transform.position = Vector3.MoveTowards(cameraController.transform.position, camPosition.position, camSpeed * Time.deltaTime);

        if (!battleEnded)
        {
            if (BossHealthController.Instance.CurrentHealth > threshold1)
            {
                if (activeCounter > 0)
                {
                    activeCounter -= Time.deltaTime;
                    if (activeCounter <= 0)
                    {
                        fadeOutCounter = fadeOutTime;
                        animator.SetTrigger("vanish");
                    }

                    shotCounter -= Time.deltaTime;
                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShots1;

                        Instantiate(bullet, shotPoint.position, Quaternion.identity);
                    }

                }
                else if (fadeOutCounter > 0)
                {
                    fadeOutCounter -= Time.deltaTime;
                    if (fadeOutCounter <= 0)
                    {
                        boss.gameObject.SetActive(false);
                        inactiveCounter = inactiveTime;
                    }
                }
                else if (inactiveCounter > 0)
                {
                    inactiveCounter -= Time.deltaTime;
                    if (inactiveCounter <= 0)
                    {
                        boss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                        boss.gameObject.SetActive(true);

                        activeCounter = activeTime;

                        shotCounter = timeBetweenShots1;

                    }
                }
            }
            else
            {
                if (targetPoint == null)
                {
                    targetPoint = boss;
                    fadeOutCounter = fadeOutTime;
                    animator.SetTrigger("vanish");
                }
                else
                {
                    if (Vector3.Distance(boss.position, targetPoint.position) > 0.02f)
                    {
                        boss.position = Vector3.MoveTowards(boss.position, targetPoint.position, moveSpeed * Time.deltaTime);
                    
                        if (Vector3.Distance(boss.position, targetPoint.position) <= 0.02f)
                        {
                            fadeOutCounter = fadeOutTime;
                            animator.SetTrigger("vanish");
                        }

                        shotCounter -= Time.deltaTime;
                        if (shotCounter <= 0)
                        {
                            if (PlayerHealthController.Instance.CurrentHealth > threshold2)
                            {
                                shotCounter = timeBetweenShots1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShots2;
                            }

                            Instantiate(bullet, shotPoint.position, Quaternion.identity);
                        }

                    }
                    else if (fadeOutCounter > 0)
                    {
                        fadeOutCounter -= Time.deltaTime;
                        if (fadeOutCounter <= 0)
                        {
                            boss.gameObject.SetActive(false);
                            inactiveCounter = inactiveTime;
                        }
                    }
                    else if (inactiveCounter > 0)
                    {
                        inactiveCounter -= Time.deltaTime;
                        if (inactiveCounter <= 0)
                        {
                            boss.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;

                            targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                            while (targetPoint.position == boss.position)
                            {
                                targetPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                            }

                            boss.gameObject.SetActive(true);

                            if (PlayerHealthController.Instance.CurrentHealth > threshold2)
                            {
                                shotCounter = timeBetweenShots1;
                            }
                            else
                            {
                                shotCounter = timeBetweenShots2;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            fadeOutCounter -= Time.deltaTime;
            if (fadeOutCounter <= 0)
            {
                if (winObjects != null)
                {
                    winObjects.SetActive(true);
                    winObjects.transform.SetParent(null);
                }

                cameraController.enabled = true;

                gameObject.SetActive(false);

                AudioManager.Instance.PlayLevelMusic();
            }
        }
    }

    public void EndBattle()
    {
        battleEnded = true;
        fadeOutCounter = fadeOutTime;

        animator.SetTrigger("vanish");

        boss.GetComponent<Collider2D>().enabled = false;

        //player won't take damage when battle over
        BossBullet[] bullets = FindObjectsOfType<BossBullet>();
        if (bullets.Length > 0 )
        {
            foreach (BossBullet bullet in bullets)
            {
                Destroy(bullet.gameObject);
            }
        }
    }
}
