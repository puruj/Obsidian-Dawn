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

    public GameObject Standing;
    public GameObject Ball;
    public float WaitToBall;
    public Animator BallAnimator;

    public bool CanMove;

    private bool isOnGround;

    private bool canDoubleJump;

    private float dashCounter;
    private float dashRechargeCounter;

    private float afterImageCounter;

    private float ballCounter;

    [SerializeField]
    private Transform bombPoint;
    [SerializeField]
    private GameObject bomb;

    private PlayerAbilityTracker playerAbilityTracker;

    // Start is called before the first frame update
    void Start()
    {
        playerAbilityTracker = GetComponent<PlayerAbilityTracker>();
        CanMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove && Time.timeScale != 0)
        {
            if (dashRechargeCounter > 0)
            {
                dashRechargeCounter -= Time.deltaTime;
            }
            else
            {
                //When standing then can dash
                if (Input.GetButtonDown("Fire2") && Standing.activeSelf && playerAbilityTracker.CanDash)
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
            if (Input.GetButtonDown("Jump") && (isOnGround || (canDoubleJump && playerAbilityTracker.CanDoubleJump)))
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

            //Shooting logic
            if (Input.GetButtonDown("Fire1"))
            {
                if (Standing.activeSelf)
                {
                    Instantiate(ShotToFire, ShotPoint.position, ShotPoint.rotation).MoveDirection =
                        new Vector2(transform.localScale.x, 0f);

                    PlayerAnimator.SetTrigger("shotFired");
                }
                else if (Ball.activeSelf && playerAbilityTracker.CanDropBomb)
                {
                    Instantiate(bomb, bombPoint.position, bombPoint.rotation);
                }
            }

            //Ball Logic
            if (!Ball.activeSelf)
            {
                if (Input.GetAxisRaw("Vertical") < -0.9f && playerAbilityTracker.CanBecomeBall)
                {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0)
                    {
                        Ball.SetActive(true);
                        Standing.SetActive(false);
                    }
                }
                else
                {
                    ballCounter = WaitToBall;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") > 0.9f)
                {
                    ballCounter -= Time.deltaTime;
                    if (ballCounter <= 0)
                    {
                        Ball.SetActive(false);
                        Standing.SetActive(true);
                    }
                }
                else
                {
                    ballCounter = WaitToBall;
                }
            }
        }
        else
        {
            PlayerRigidBody.velocity = Vector2.zero;
        }

        if (Standing.activeSelf)
        {
            PlayerAnimator.SetBool("isOnGround", isOnGround);
            PlayerAnimator.SetFloat("speed", Mathf.Abs(PlayerRigidBody.velocity.x));
        }
        if (Ball.activeSelf)
        {
            BallAnimator.SetFloat("speed", Mathf.Abs(PlayerRigidBody.velocity.x));
        }
    }

    public void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(PlayerAfterImageSpriteRenderer, transform.position, transform.rotation);

        image.sprite = PlayerSpriteRenderer.sprite;
        image.transform.localScale = transform.localScale;
        image.color = AfterImageColor;

        Destroy(image.gameObject, AfterImageLifeTime);

        afterImageCounter = TimeBetweenAfterImages;
    }

}
