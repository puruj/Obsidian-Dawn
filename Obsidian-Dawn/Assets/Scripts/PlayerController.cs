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

    public float DashSpeed;
    public float DashTime;
    public float WaitAfterDashing;

    public SpriteRenderer PlayerSpriteRenderer;
    public SpriteRenderer PlayerAfterImageSpriteRenderer;
    public float AfterImageLifeTime;
    public float TimeBetweenAfterImages;
    public Color AfterImageColor;

    private bool isOnGround;

    private bool canDoubleJump;

    private float dashCounter;
    private float dashRechargeCounter;

    private float afterImageCounter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetButtonDown("Fire2"))
            {
                dashCounter = DashTime;
                ShowAfterImage();
            }
        }

        if (dashCounter > 0)
        {
            dashCounter = dashCounter - Time.deltaTime;

            PlayerRigidBody.velocity = new Vector2(DashSpeed * transform.localScale.x, PlayerRigidBody.velocity.y);

            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter <= 0)
            {
                ShowAfterImage();
            }

            dashRechargeCounter = WaitAfterDashing;
        }
        else
        {
            //move sideways
            PlayerRigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * MoveSpeed, PlayerRigidBody.velocity.y);

            //handle direction change
            if (PlayerRigidBody.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (PlayerRigidBody.velocity.y > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }


        //ground check
        isOnGround = Physics2D.OverlapCircle(GroundPoint.position, 0.2f, WhatIsGround);

        //jump check
        if (Input.GetButtonDown("Jump") && (isOnGround || canDoubleJump))
        {
            if (isOnGround)
            {
                canDoubleJump = true;               
            }
            else
            {
                canDoubleJump = false;
                PlayerAnimator.SetTrigger("doubleJump");
            }

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

    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(PlayerAfterImageSpriteRenderer, transform.position, transform.rotation);

        image.sprite = PlayerSpriteRenderer.sprite;
        image.transform.localScale = transform.localScale;
        image.color = AfterImageColor;

        Destroy(image, AfterImageLifeTime);

        afterImageCounter = TimeBetweenAfterImages;
    }

}
