using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float BulletSpeed;
    public Rigidbody2D BulletRigidbody;

    public Vector2 MoveDirection;

    public GameObject ImpactEffect;

    // Update is called once per frame
    void Update()
    {
        BulletRigidbody.velocity = MoveDirection * BulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(ImpactEffect != null)
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
