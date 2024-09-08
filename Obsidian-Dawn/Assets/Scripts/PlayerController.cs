using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D PlayerRigidBody;

    public float MoveSpeed;
    public float JumpForce;

    public Transform GroundPoint;
    public LayerMask WhatIsGround;

    public Animator PlayerAnimator;

    public BulletController ShotToFire;
    public Transform ShotPoint;


    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move sideways
        PlayerRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, PlayerRigidBody.velocity.y);

        //handle direction change
        if(PlayerRigidBody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(PlayerRigidBody.velocity.y > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        //ground check
        isOnGround = Physics2D.OverlapCircle(GroundPoint.position, 0.2f, WhatIsGround);

        //jump check
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, JumpForce);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(ShotToFire, ShotPoint.position, ShotPoint.rotation).MoveDirection =
                new Vector2(transform.localScale.x, 0f);

            PlayerAnimator.SetTrigger("shotFired");
        }

        PlayerAnimator.SetBool("isOnGround", isOnGround);
        PlayerAnimator.SetFloat("speed", Mathf.Abs(PlayerRigidBody.velocity.x));
    }
}
