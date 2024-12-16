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

    bool hasControlAndPhysics = false;

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float doubleJumpSpeed;
    float xInput;
    float yInput;
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

    bool isKnocked;
    bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        RespawnFinished(false);
        UpdateSkin();
    }

    void Update()
    {
        HandleAnimation();
        if (isKnocked || !hasControlAndPhysics) return;  // Prevent movemnt of any kind while knocked

        HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleEnemyDetection();
        HandleFlip();
        // There is a very very small glitch with player when exiting the wall slide anim, to fix that keep below HandleFlip()
        HandleCollision();
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
        Destroy(gameObject);
        Instantiate(playerDeathVfx, transform.position, Quaternion.identity);
    }

    void HandleWallSlide()
    {
        float yVelocityModifier = yInput < 0 ? 1f : 0.5f;
        if (isWallDetected && rb.linearVelocity.y < 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yVelocityModifier);
    }

    void HandleFlip()
    {
        if ((xInput < 0 && facingRight) || (xInput > 0 && !facingRight)) Flip();
    }

    void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKeyDown(KeyCode.Space);
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(xInput, jumpSpeed);
        canDoubleJump = true;
    }

    void DoubleJump() => rb.linearVelocity = new Vector2(xInput, doubleJumpSpeed);

    void WallJump()
    {
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
            rb.linearVelocity = new Vector2(xInput * speed, rb.linearVelocity.y);
        }
        if (isWallDetected || isOnGround)
        {
            isWallJumping = false;
            wentOffTheGroundAt = -1;
        }
        if (!isOnGround && rb.linearVelocity.y < 0 && wentOffTheGroundAt == -1) wentOffTheGroundAt = Time.time;
        if (jumpInput)
        {
            //print("Input:- ");
            if (isOnGround)
            {
                //print("Normal Jump!!");
                Jump();
            }
            else if (isWallDetected)
            {
                //print("Wall Jump!!");
                WallJump();
            }
            else if (canDoubleJump)
            {
                //print("Double Jump!!");
                isWallJumping = false;
                DoubleJump();
                canDoubleJump = false;
            }
            else
            {
                bufferInputReceived = Time.time;
                if (Time.time < wentOffTheGroundAt + coyoteJumpWindow)
                {
                    //print("Coyote Jump!!");
                    wentOffTheGroundAt = -1;
                    Jump();
                }
            }
            wentOffTheGroundAt = -2;
        }
        if (isOnGround && Time.time < bufferInputReceived + bufferJumpWindow)
        {
            //print("Buffered Jump!!");
            bufferInputReceived = -1;   // Should be set to <= -bufferJumpWindow
            Jump();
        }
    }

    public void KnockBack(Transform damageTransform, Vector2 knockBackSpeed, float knockBackDuration)
    {
        if (isKnocked) return;
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
