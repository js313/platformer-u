using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D cd;
    Animator anim;

    GameManager gameManager;
    DifficultyManager difficultyManager;

    PlayerInput playerInput;

    bool hasControlAndPhysics = false;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float doubleJumpSpeed;
    Vector2 moveInput;
    bool jumpInput;
    bool canDoubleJump = true;

    [Header("Collision")]
    [SerializeField] float groundCheckDistance;
    [SerializeField] float wallCheckDistance;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] Transform enemyDetection;
    [SerializeField] float enemyDetectionRadius;
    bool isOnGround;

    [Header("Wall")]
    [SerializeField] Vector2 wallJumpSpeed;
    bool isWallJumping;
    bool isWallDetected, wallDetectionStopped = false;

    [Header("Buffer Jump")]
    [SerializeField] float bufferJumpWindow;
    float bufferInputReceived = -1; // Set to <= -bufferJumpWindow

    [Header("Coyote Jump")]
    [SerializeField] float coyoteJumpWindow;
    float wentOffTheGroundAt = -1; // Set to <= -coyoteJumpWindow

    [Header("VFX")]
    [SerializeField] GameObject playerDeathVfx;
    [SerializeField] AnimatorOverrideController[] controllers;
    [SerializeField] ParticleSystem dustVfx;

    bool isKnocked;
    bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();

        playerInput.Player.Jump.performed += ctx => HandleJump();
        playerInput.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        playerInput.Disable();

        playerInput.Player.Jump.performed -= ctx => HandleJump();
        playerInput.Player.Movement.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Movement.canceled -= ctx => moveInput = Vector2.zero;
    }

    void Start()
    {
        difficultyManager = DifficultyManager.instance;
        gameManager = GameManager.instance;
        RespawnFinished(false);
        UpdateSkin();
    }

    void Update()
    {
        if (gameManager.isGamePaused) return;
        HandleAnimation();
        if (isKnocked || !hasControlAndPhysics) return;  // Prevent movemnt of any kind while knocked

        // HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleEnemyDetection();
        HandleFlip();
        // There is a very very small glitch with player when exiting the wall slide anim, to fix that keep below HandleFlip()
        HandleCollision();
    }

    public void Damage()
    {
        if (difficultyManager == null || isKnocked) return;

        if (difficultyManager.difficulty == DifficultyType.Normal)
        {
            if (gameManager.fruitsCollected > 0)
                gameManager.FruitDropped();
            else
            {
                Die();
                gameManager.RestartLevel();
            }
        }
        else if (difficultyManager.difficulty == DifficultyType.Hard)
        {
            Die();
            gameManager.RestartLevel();
        }
    }

    void UpdateSkin()
    {
        if (SkinManager.instance != null)
        {
            anim.runtimeAnimatorController = controllers[SkinManager.instance.selectedSkinIndex];
        }
    }

    void HandleEnemyDetection()
    {
        if (rb.linearVelocity.y >= 0) return;

        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(enemyDetection.position, enemyDetectionRadius, whatIsEnemy);

        foreach (Collider2D enemyCollider in enemyColliders)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy == null) continue;
            AudioManager.instance.PlaySfx(1);

            enemy.Die();
            Jump();
        }
    }

    public void RespawnFinished(bool isFinished)
    {
        if (!isFinished)
        {
            hasControlAndPhysics = false;
            rb.bodyType = RigidbodyType2D.Static;
            cd.enabled = false;
        }
        else
        {
            hasControlAndPhysics = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            cd.enabled = true;
        }
    }

    public void Die()
    {
        AudioManager.instance.PlaySfx(0, false);

        Destroy(gameObject);
        Instantiate(playerDeathVfx, transform.position, Quaternion.identity);
    }

    void HandleWallSlide()
    {
        float yVelocityModifier = moveInput.y < 0 ? 1f : 0.5f;
        if (isWallDetected && rb.linearVelocity.y < 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yVelocityModifier);
    }

    void HandleFlip()
    {
        if ((moveInput.x < 0 && facingRight) || (moveInput.x > 0 && !facingRight)) Flip();
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    void HandleInput()
    {
        // xInput = Input.GetAxisRaw("Horizontal");
        // yInput = Input.GetAxisRaw("Vertical");
        // jumpInput = Input.GetKeyDown(KeyCode.Space);
    }

    void Jump()
    {
        dustVfx.Play();
        AudioManager.instance.PlaySfx(3);

        rb.linearVelocity = new Vector2(moveInput.x, jumpSpeed);
        canDoubleJump = true;
    }

    void DoubleJump()
    {
        //dustVfx.Play();
        AudioManager.instance.PlaySfx(3);

        rb.linearVelocity = new Vector2(moveInput.x, doubleJumpSpeed);
    }

    void WallJump()
    {
        AudioManager.instance.PlaySfx(12);

        isWallJumping = true;
        Flip();
        rb.linearVelocity += new Vector2(wallJumpSpeed.x * Math.Sign(transform.right.x), wallJumpSpeed.y);
        StartCoroutine(StopWallDetection());
        canDoubleJump = true;
    }

    IEnumerator StopWallDetection()
    {
        float wallDetectionBufferAfterWallJumpInitiated = 0.05f;
        wallDetectionStopped = true;
        yield return new WaitForSeconds(wallDetectionBufferAfterWallJumpInitiated);
        wallDetectionStopped = false;
    }

    void HandleCollision()
    {
        isOnGround = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = !wallDetectionStopped && Physics2D.Raycast(transform.position, transform.right, wallCheckDistance, whatIsGround);
    }

    void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isOnGround", isOnGround);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    void HandleMovement()
    {
        if (!isWallDetected && !isWallJumping)
        {
            rb.linearVelocity = new Vector2(moveInput.x * speed, rb.linearVelocity.y);
        }
        if (isWallDetected || isOnGround)
        {
            if (isOnGround && wentOffTheGroundAt != -1) dustVfx.Play();
            wentOffTheGroundAt = -1;
        }
        if (isWallJumping && isOnGround) isWallJumping = false;
        if (!isOnGround && rb.linearVelocity.y < 0 && wentOffTheGroundAt == -1) wentOffTheGroundAt = Time.time;

        if (isOnGround && Time.time < bufferInputReceived + bufferJumpWindow)
        {
            // print("Buffered Jump!!");
            bufferInputReceived = -1;   // Should be set to <= -bufferJumpWindow
            Jump();
        }
    }

    void HandleJump()
    {
        if (gameManager.isGamePaused) return;
        if (isOnGround)
        {
            // print("Normal Jump!!");
            Jump();
        }
        else if (isWallDetected && !isWallJumping)
        {
            // print("Wall Jump!!");
            WallJump();
        }
        else if (canDoubleJump)
        {
            // print("Double Jump!!");
            isWallJumping = false;
            DoubleJump();
            canDoubleJump = false;
        }
        else
        {
            bufferInputReceived = Time.time;
            if (Time.time < wentOffTheGroundAt + coyoteJumpWindow)
            {
                // print("Coyote Jump!!");
                wentOffTheGroundAt = -1;
                Jump();
            }
            wentOffTheGroundAt = -2;
        }
    }

    public void KnockBack(Transform damageTransform, Vector2 knockBackSpeed, float knockBackDuration)
    {
        if (isKnocked) return;
        CameraManager.instance.ScreenShake();
        StartCoroutine(KnockBackRoutine(knockBackDuration));
        Vector2 damageDirection = transform.position - damageTransform.position;
        rb.linearVelocity = new Vector2(knockBackSpeed.x * Math.Sign(damageDirection.x), knockBackSpeed.y);
    }

    IEnumerator KnockBackRoutine(float knockBackDuration)
    {
        isKnocked = true;
        anim.SetBool("knockback", true);
        yield return new WaitForSeconds(knockBackDuration);
        anim.SetBool("knockback", false);
        isKnocked = false;
    }

    public void Push(Vector2 pushDirection, float pushForce, float pushDuration)
    {
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(PushCoroutine(pushDirection, pushForce, pushDuration));
    }

    IEnumerator PushCoroutine(Vector2 pushDirection, float pushForce, float pushDuration)
    {
        hasControlAndPhysics = false;
        rb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(pushDuration);
        hasControlAndPhysics = true;
    }

    void OnDrawGizmos()
    {
        // Enemy Damaging Sphere
        Gizmos.DrawWireSphere(enemyDetection.position, enemyDetectionRadius);
        // Ground Check Ray
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        // Wall Check Ray
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + wallCheckDistance * (facingRight ? 1 : -1), transform.position.y));
    }
}
