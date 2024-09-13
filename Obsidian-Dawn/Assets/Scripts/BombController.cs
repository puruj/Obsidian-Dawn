using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public GameObject Explosion;
    [SerializeField]
    private float timeToExplode = 0.5f;

    public float BlastRange;
    public LayerMask WhatIsDestructibleMask;

    public int damageAmount;
    public LayerMask WhatIsDamageable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToExplode -= Time.deltaTime;
        if (timeToExplode <= 0)
        {
            if (Explosion != null)
            {
                Instantiate(Explosion, transform.position, transform.rotation);
            }

            Destroy(gameObject);

            Collider2D[] objectsToRemove = Physics2D.OverlapCircleAll(transform.position, BlastRange, WhatIsDestructibleMask);

            if (objectsToRemove.Length > 0)
            {
                foreach (Collider2D objectToRemove in objectsToRemove)
                {
                    Destroy(objectToRemove.gameObject);
                }
            }

            Collider2D[] objectsToDamage = Physics2D.OverlapCircleAll(transform.position, BlastRange, WhatIsDamageable);

            foreach (Collider2D col in objectsToDamage)
            {
                EnemyHealthController enemyHealth = col.GetComponent<EnemyHealthController>();
                if (enemyHealth != null)
                {
                    enemyHealth.DamageEnemy(damageAmount);
                }
            }

            AudioManager.Instance.PlaySFXAdjusted(4);
        }
    }
}
