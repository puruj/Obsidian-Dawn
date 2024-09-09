using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField]
    private int totalHealth = 3;

    [SerializeField]
    private GameObject deathEffect;

    public void DamageEnemy(int damageAmount)
    {
        totalHealth -= Mathf.Abs(damageAmount);

        if (totalHealth <= 0)
        {
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }

    }

}
