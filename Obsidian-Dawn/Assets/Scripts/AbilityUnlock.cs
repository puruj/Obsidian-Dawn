using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AbilityUnlock : MonoBehaviour
{
    [SerializeField]
    private bool unlockDoubleJump;
    [SerializeField]
    private bool unlockDash;
    [SerializeField]
    private bool unlockBecomeBall;
    [SerializeField]
    private bool unlockDropBomb;

    [SerializeField] 
    private GameObject pickUpEffect;

    [SerializeField]
    private TMP_Text unlockText;

    [SerializeField]
    private string unlockMessage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerAbilityTracker playerAbilityTracker = collision.GetComponentInParent<PlayerAbilityTracker>();

            if (playerAbilityTracker != null)
            {
                if (unlockDoubleJump)
                {
                    playerAbilityTracker.CanDoubleJump = true;
                }

                if (unlockDash)
                {
                    playerAbilityTracker.CanDash = true;
                }

                if (unlockBecomeBall)
                {
                    playerAbilityTracker.CanBecomeBall = true;
                }

                if (unlockDropBomb)
                {
                    playerAbilityTracker.CanDropBomb = true;
                }
            }

            Instantiate(pickUpEffect, transform.position, Quaternion.identity);

            unlockText.transform.parent.SetParent(null);
            unlockText.transform.parent.position = transform.position;

            unlockText.text = unlockMessage;
            unlockText.gameObject.SetActive(true);

            Destroy(unlockText.transform.parent.gameObject, 5f);

            Destroy(gameObject);
        }
    }
}
