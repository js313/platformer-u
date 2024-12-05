using System;
using System.Collections;
using System.Collections.Generic;
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
    bool isOnGround;

    [Header("Wall")]
    [SerializeField] Vector2 wallJumpSpeed;
    private bool isWallJumping;
    bool isWallDetected, wallDetectionStopped = false;

    [Header("Knockback")]
    [SerializeField] float knockBackDuration;
    [SerializeField] Vector2 knockBackSpeed;
    bool isKnocked;

    [Header("Buffer Jump")]
    [SerializeField] float bufferJumpWindow;
    float bufferInputReceived = -1; // Set to <= -bufferJumpWindow

    [Header("Coyote Jump")]
    [SerializeField] float coyoteJumpWindow;
    float wentOffTheGroundAt = -1; // Set to <= -coyoteJumpWindow

    [Header("VFX")]
    [SerializeField] GameObject playerDeathVfx;

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
    }

    void Update()
    {
        if (isKnocked || !hasControlAndPhysics) return;  // Prevent movemnt of any kind while knocked

        HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        // There is a very very small glitch with player when exiting the wall slide anim, to fix that keep below HandleFlip()
        HandleCollision();
        HandleAnimation();
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

    public void KnockBack()
    {
        if (isKnocked) return;
        StartCoroutine(KnockBackRoutine());
        rb.velocity = new Vector2(knockBackSpeed.x * -Math.Sign(transform.right.x), knockBackSpeed.y);
        anim.SetTrigger("knockback");
    }

    public void Die()
    {
        Destroy(gameObject);
        Instantiate(playerDeathVfx, transform.position, Quaternion.identity);
    }

    IEnumerator KnockBackRoutine()
    {
        isKnocked = true;
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }

    void HandleWallSlide()
    {
        float yVelocityModifier = yInput < 0 ? 1f : 0.5f;
        if (isWallDetected && rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yVelocityModifier);
        }
    }

    private void HandleFlip()
    {
        if ((xInput < 0 && facingRight) || (xInput > 0 && !facingRight)) Flip();
    }

    private void Flip()
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

    private void Jump()
    {
        rb.velocity = new Vector2(xInput, jumpSpeed);
        canDoubleJump = true;
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector2(xInput, doubleJumpSpeed);
    }

    private void WallJump()
    {
        isWallJumping = true;
        Flip();
        rb.velocity += new Vector2(wallJumpSpeed.x * Math.Sign(transform.right.x), wallJumpSpeed.y);
        StartCoroutine(StopWallDetection());
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
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isOnGround", isOnGround);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    void HandleMovement()
    {
        if (!isWallDetected && !isWallJumping)
        {
            rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
        }
        if (isWallDetected || isOnGround)
        {
            isWallJumping = false;
            wentOffTheGroundAt = -1;
        }
        if (!isOnGround && rb.velocity.y < 0 && wentOffTheGroundAt == -1) wentOffTheGroundAt = Time.time;
        if (jumpInput)
        {
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
            else if (canDoubleJump) // BUG: Something wrong with Double Jump right after wall jump
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

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + wallCheckDistance, transform.position.y));
    }
}
