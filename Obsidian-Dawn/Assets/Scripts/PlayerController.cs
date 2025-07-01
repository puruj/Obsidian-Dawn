using System.Collections;
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

    private bool isOnGround;
    private bool canDoubleJump;
    private bool jumpRequested;
    private bool dashRequested;
    private float horizontalInput;
    private float dashCounter;
    private float dashRechargeCounter;
    private float afterImageCounter;
    private float ballCounter;

    private PlayerAbilityTracker _abilityTracker;
    private static readonly Queue<SpriteRenderer> afterImagePool = new();
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
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }
        if (Input.GetButtonDown("Fire2"))
        {
            dashRequested = true;
        }
        // bullets / bombs
        HandleFireInput();
        // ball transistion
        HandleBallMorphInput();    
        UpdateAnimators();     
    }
    /// <summary>
    /// Fixed-time-step physics logic: ground check, dash, move, jump.
    /// </summary>
    private void FixedUpdate()
    {
        if (!CanMove || Time.timeScale == 0) 
        { 
            PlayerRigidBody.velocity = Vector2.zero; 
            return; 
        }

        // Ground check first so jump logic is correct
        isOnGround = Physics2D.OverlapCircle(GroundPoint.position, 0.2f, WhatIsGround);

        HandleDash();
        HandleHorizontalMovement();
        HandleJump();
    }
    #endregion

    /// <summary>
    /// Consumes dash request, applies burst velocity, drops after-images,
    /// and lets designer set recharge time.
    /// </summary>
    private void HandleDash()
    {
        // recharge timer 
        if (dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.fixedDeltaTime;
        }

        // queue dash
        if (dashRequested && Standing.activeSelf && _abilityTracker.CanDash && dashRechargeCounter <= 0)
        {
            dashCounter = DashTime;
            dashRechargeCounter = WaitAfterDashing;
            ShowAfterImage();
            AudioManager.Instance.PlaySFXAdjusted(7);
        }
        // consume request
        dashRequested = false;   

        // active dash movement
        if (dashCounter > 0)
        {
            dashCounter -= Time.fixedDeltaTime;
            PlayerRigidBody.velocity = new Vector2(DashSpeed * transform.localScale.x, PlayerRigidBody.velocity.y);

            afterImageCounter -= Time.fixedDeltaTime;
            if (afterImageCounter <= 0) ShowAfterImage();
            // skip normal movement while dashing
            return; 
        }
    }

    private void HandleHorizontalMovement()
    {
        PlayerRigidBody.velocity = new Vector2(horizontalInput * MoveSpeed, PlayerRigidBody.velocity.y);

        // flip sprite
        if (horizontalInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (horizontalInput > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    /// <summary>
    /// Single or double jump depending on ground state and ability flags.
    /// </summary>
    private void HandleJump()
    {
        // nothing queued
        if (!jumpRequested)
        {
            return;  
        }
        jumpRequested = false;

        if (isOnGround || (canDoubleJump && _abilityTracker.CanDoubleJump))
        {
            if (isOnGround)
            {
                canDoubleJump = true;
                AudioManager.Instance.PlaySFXAdjusted(12);
            }
            else
            {
                canDoubleJump = false;
                PlayerAnimator.SetTrigger("doubleJump");
                AudioManager.Instance.PlaySFXAdjusted(9);
            }
            PlayerRigidBody.velocity = new Vector2(PlayerRigidBody.velocity.x, JumpForce);
        }
    }
    /// <summary>
    /// Fires a bullet in standing mode, or drops a bomb while in ball mode.
    /// </summary>
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
    /// <summary>
    /// Handles hold-down input for morphing between standing and ball forms.
    /// Uses a small delay so accidental taps don’t toggle state.
    /// </summary>
    private void HandleBallMorphInput()
    {
        // STANDING TO BALL
        if (!Ball.activeSelf)          
        {
            if (Input.GetAxisRaw("Vertical") < -0.9f && _abilityTracker.CanBecomeBall)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    Ball.SetActive(true); Standing.SetActive(false);
                    AudioManager.Instance.PlaySFX(6);
                }
            }
            else ballCounter = WaitToBall;
        }
        // BALL TO STANDING
        else
        {
            if (Input.GetAxisRaw("Vertical") > 0.9f)
            {
                ballCounter -= Time.deltaTime;
                if (ballCounter <= 0)
                {
                    Ball.SetActive(false); Standing.SetActive(true);
                    AudioManager.Instance.PlaySFX(10);
                }
            }
            else ballCounter = WaitToBall;
        }
    }
    /// <summary>
    /// Pushes velocity / ground state to the correct animator so
    /// art can update independently of gameplay code.
    /// </summary>
    private void UpdateAnimators()
    {
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
    /// <summary>
    /// Spawns or re-uses a sprite trail element for the dash effect.
    /// Pulls from a small pool to avoid garbage allocations.
    /// </summary>
    private void ShowAfterImage()
    {
        //Pop until we find a live object
        SpriteRenderer image = null;
        while (afterImagePool.Count > 0 && image == null)
        {
            var candidate = afterImagePool.Dequeue();
            // Unity null check (handles destroyed objects)
            if (candidate != null)
            {
                image = candidate;
            }            
        }

        // Create new if needed
        if (image == null)
        {
            image = Instantiate(PlayerAfterImageSpriteRenderer);
        }

        //Re-initialise
        image.transform.SetPositionAndRotation(transform.position, transform.rotation);
        image.transform.localScale = transform.localScale;
        image.sprite = PlayerSpriteRenderer.sprite;
        image.color = AfterImageColor;
        image.gameObject.SetActive(true);

        StartCoroutine(DisableAfterLifeTime(image));
        afterImageCounter = TimeBetweenAfterImages;
    }

    /// <summary>
    /// Waits for the configured lifetime, hides the image, and
    /// pushes it back into the queue for instant reuse next dash.
    /// </summary>
    IEnumerator DisableAfterLifeTime(SpriteRenderer img)
    {
        yield return new WaitForSeconds(AfterImageLifeTime);
        img.gameObject.SetActive(false);
        // recycled, zero allocation next time
        afterImagePool.Enqueue(img);           
    }

}
