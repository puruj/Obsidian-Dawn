using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int DamageAmount = 1;

    public bool DestroyOnDamage;
    public GameObject DestroyEffect;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            DealDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            DealDamage();
        }
    }

    private void DealDamage()
    {
        PlayerHealthController.Instance.DamagePlayer(DamageAmount);

        if (DestroyOnDamage)
        {
            if(DestroyEffect != null)
            {
                Instantiate(DestroyEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }

}
