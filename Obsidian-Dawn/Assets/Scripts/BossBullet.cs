using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Rigidbody2D bossBulletRigidBody;
    [SerializeField]
    public int damageAmount;
    [SerializeField]
    private GameObject impactEffect;


    // Start is called before the first frame update
    void Start()
    {
        Vector3 direction = transform.position - PlayerHealthController.Instance.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        bossBulletRigidBody.velocity = -transform.right * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealthController.Instance.DamagePlayer(damageAmount);
        }

        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        
    }
}
