using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyingController : MonoBehaviour
{
    public float RangeToStartChase;
    
    public float MoveSpeed;
    public float TurnSpeed;

    public Animator EnemyFlyingAnimator;

    private bool isChasing;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.Instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isChasing)
        {
            if (Vector3.Distance(transform.position, player.position) < RangeToStartChase)
            {
                isChasing = true;

                EnemyFlyingAnimator.SetBool("isChasing", isChasing);
            }
        }
        else
        {
            if (player.gameObject.activeSelf)
            {
                Vector3 direction = transform.position - player.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);

                transform.position += -transform.right * MoveSpeed * Time.deltaTime;
            }
        }
    }
}
