using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatroller : MonoBehaviour
{

    [SerializeField]
    private Transform[] patrolPoints;
    private int currentPoint;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float waitAtPoints;
    private float waitCounter;

    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private Rigidbody2D rigidBody;

    [SerializeField]
    private Animator enemyPatrollerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        waitCounter = waitAtPoints;

        foreach (var point in patrolPoints)
        {
            point.SetParent(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > 0.2f)
        {
            if (transform.position.x < patrolPoints[currentPoint].position.x)
            {
                rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                rigidBody.velocity = new Vector2(-moveSpeed, rigidBody.velocity.y);
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            if (transform.position.y < patrolPoints[currentPoint].position.y - 0.5f && rigidBody.velocity.y < 0.1f)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            }

        }
        else
        {
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);

            waitCounter -= Time.deltaTime;

            if (waitCounter <= 0)
            {
                waitCounter = waitAtPoints;

                currentPoint++;

                if (currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }

        enemyPatrollerAnimator.SetFloat("speed", Mathf.Abs(rigidBody.velocity.x));
    }
}
