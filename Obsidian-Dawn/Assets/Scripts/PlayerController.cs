using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Refs")]
    public Rigidbody2D PlayerRigidBody;
    public Animator PlayerAnimator;
    public Animator BallAnimator;
    public SpriteRenderer PlayerSpriteRenderer;
    public SpriteRenderer PlayerAfterImageSpriteRenderer;
    public Transform GroundPoint;
    public Transform ShotPoint;
    public Transform bombPoint;
    public BulletController ShotToFire;
    public GameObject Standing;
    public GameObject Ball;
    public GameObject bomb;
    public LayerMask WhatIsGround;

    [Header("Move / Jump")]
    public float MoveSpeed = 7f;
    public float JumpForce = 15f;

    [Header("Dash")]
    public float DashSpeed = 20f;
    public float DashTime = .25f;
    public float WaitAfterDashing = .5f;

    [Header("After-image")]
    public float AfterImageLifeTime = .3f;
    public float TimeBetweenAfterImages = .05f;
    public Color AfterImageColor;

    [Header("Ball morph")]
    public float WaitToBall = .5f;

    private bool _isOnGround;
    private bool _canDoubleJump;
    private bool _jumpRequested;
    private bool _dashRequested;
    private float _horizontalInput;
    private float _dashCounter;
    private float _dashRechargeCounter;
    private float _afterImageCounter;
    private float _ballCounter;

    private PlayerAbilityTracker _abilityTracker;
    public bool CanMove { get; set; } = true;

    #region Unity Life-cycle
    private void Start()
    {
        _abilityTracker = GetComponent<PlayerAbilityTracker>();
    }

    private void Update()
    {
        if (!CanMove || Time.timeScale == 0) 
        {
            return; 
        }

        //Poll input
        _horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            _jumpRequested = true;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            _dashRequested = true;
        }

        HandleFireInput();         // bullets / bombs
        HandleBallMorphInput();    // ball transistion
        UpdateAnimators();         // purely visual
    }

    private void FixedUpdate()
    {
        if (!CanMove || Time.timeScale == 0) 
        { 
            PlayerRigidBody.velocity = Vector2.zero; 
            return; 
        }

        // Ground check first so jump logic is correct
        _isOnGround = Physics2D.OverlapCircle(GroundPoint.position, 0.2f, WhatIsGround);

        HandleDash();
        HandleHorizontalMovement();
        HandleJump();
    }
    #endregion

    private void HandleDash()
    {
        // recharge timer 
        if (_dashRechargeCounter > 0)
        {
            _dashRechargeCounter -= Time.fixedDeltaTime;
        }

        // queue dash
        if (_dashRequested && Standing.activeSelf && _abilityTracker.CanDash && _dashRechargeCounter <= 0)
        {
            _dashCounter = DashTime;
            _dashRechargeCounter = WaitAfterDashing;
            ShowAfterImage();
            AudioManager.Instance.PlaySFXAdjusted(7);
        }
        _dashRequested = false;   // consume request

        // active dash movement
        if (_dashCounter > 0)
        {
            _dashCounter -= Time.fixedDeltaTime;
            PlayerRigidBody.velocity = new Vector2(DashSpeed * transform.localScale.x, PlayerRigidBody.velocity.y);

            _afterImageCounter -= Time.fixedDeltaTime;
            if (_afterImageCounter <= 0) ShowAfterImage();
            return; // skip normal movement while dashing
        }
    }

    private void HandleHorizontalMovement()
    {
        PlayerRigidBody.velocity = new Vector2(_horizontalInput * MoveSpeed, PlayerRigidBody.velocity.y);

        // flip sprite
        if (_horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (_horizontalInput > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void HandleJump()
    {
        // nothing queued
        if (!_jumpRequested)
        {
            return;  
        }
        _jumpRequested = false;

        if (_isOnGround || (_canDoubleJump && _abilityTracker.CanDoubleJump))
        {
            if (_isOnGround)
            {
                _canDoubleJump = true;
                AudioManager.Instance.PlaySFXAdjusted(12);
            }
            else
            {
                _canDoubleJump = false;
                PlayerAnimator.SetTrigger("doubleJump");
                AudioManager.Instance.PlaySFXAdjusted(9);
            }
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, JumpForce);
        }
    }

    private void HandleFireInput()
    {
        if (!Input.GetButtonDown("Fire1")) return;

        if (Standing.activeSelf)
        {
            Instantiate(ShotToFire, ShotPoint.position, ShotPoint.rotation)
                .MoveDirection = new Vector2(transform.localScale.x, 0f);

            PlayerAnimator.SetTrigger("shotFired");
            AudioManager.Instance.PlaySFXAdjusted(14);
        }
        else if (Ball.activeSelf && _abilityTracker.CanDropBomb)
        {
            Instantiate(bomb, bombPoint.position, bombPoint.rotation);
            AudioManager.Instance.PlaySFXAdjusted(13);
        }
    }

    private void HandleBallMorphInput()
    {
        if (!Ball.activeSelf)          // STANDING TO BALL
        {
            if (Input.GetAxisRaw("Vertical") < -0.9f && _abilityTracker.CanBecomeBall)
            {
                _ballCounter -= Time.deltaTime;
                if (_ballCounter <= 0)
                {
                    Ball.SetActive(true); Standing.SetActive(false);
                    AudioManager.Instance.PlaySFX(6);
                }
            }
            else _ballCounter = WaitToBall;
        }
        else                           // BALL TO STANDING
        {
            if (Input.GetAxisRaw("Vertical") > 0.9f)
            {
                _ballCounter -= Time.deltaTime;
                if (_ballCounter <= 0)
                {
                    Ball.SetActive(false); Standing.SetActive(true);
                    AudioManager.Instance.PlaySFX(10);
                }
            }
            else _ballCounter = WaitToBall;
        }
    }

    private void UpdateAnimators()
    {
        if (Standing.activeSelf)
        {
            PlayerAnimator.SetBool("isOnGround", _isOnGround);
            PlayerAnimator.SetFloat("speed", Mathf.Abs(PlayerRigidBody.velocity.x));
        }
        if (Ball.activeSelf)
        {
            BallAnimator.SetFloat("speed", Mathf.Abs(PlayerRigidBody.velocity.x));
        }
    }

    private void ShowAfterImage()
    {
        var image = Instantiate(PlayerAfterImageSpriteRenderer, transform.position, transform.rotation);
        image.sprite = PlayerSpriteRenderer.sprite;
        image.transform.localScale = transform.localScale;
        image.color = AfterImageColor;

        Destroy(image.gameObject, AfterImageLifeTime);
        _afterImageCounter = TimeBetweenAfterImages;
    }
}
