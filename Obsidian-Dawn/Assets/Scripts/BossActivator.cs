using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField]
    private GameObject BossToActivate;

    [SerializeField]
    private string bossRef;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (PlayerPrefs.HasKey(bossRef))
            {
                if(PlayerPrefs.GetInt(bossRef) != 1)
                {
                    BossToActivate.SetActive(true);
                    gameObject.SetActive(false);
                }
            }
            else
            {
                BossToActivate.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
