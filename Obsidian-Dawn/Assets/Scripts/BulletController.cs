using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Vector2 MoveDirection;

    [SerializeField]
    private float BulletSpeed;
    [SerializeField]
    private Rigidbody2D BulletRigidbody;

    [SerializeField]
    private GameObject ImpactEffect;

    [SerializeField]
    private int damageAmount = 1;

    // Update is called once per frame
    void Update()
    {
        BulletRigidbody.velocity = MoveDirection * BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }

        if (other.tag == "Boss")
        {
            BossHealthController.Instance.TakeDamage(damageAmount);
        }

        if (ImpactEffect != null)
        {
            Instantiate(ImpactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
