using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D RB;

    public float MoveSpeed;
    public float JumpForce;

    public Transform GroundPoint;
    public LayerMask WhatIsGround;

    public Animator PlayerAnimator;

    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move sideways
        RB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, RB.velocity.y);

        //handle direction change
        if(RB.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(RB.velocity.y > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        //ground check
        isOnGround = Physics2D.OverlapCircle(GroundPoint.position, 0.2f, WhatIsGround);

        //jump check
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            RB.velocity = new Vector2(RB.velocity.x, JumpForce);
        }


        PlayerAnimator.SetBool("isOnGround", isOnGround);
        PlayerAnimator.SetFloat("speed", Mathf.Abs(RB.velocity.x));
    }
}
