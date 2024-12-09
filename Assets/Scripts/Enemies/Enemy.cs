using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;

    [SerializeField] protected float moveSpeed;

    [SerializeField] protected bool facingRight = false;

    [Header("Collision")]
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected float wallCheckDistance = 1;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheckOrigin;

    protected bool isOnGround = true;
    protected bool isGroundAhead = true;
    protected bool isFacingWall = false;

    protected bool isIdle = false;
    [SerializeField] protected float idleDuration;
    protected float idleTime;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        HandleCollision();
    }

    protected void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    void HandleCollision()
    {
        isOnGround = Physics2D.Raycast(transform.position, -transform.up, groundCheckDistance, whatIsGround);
        isGroundAhead = Physics2D.Raycast(groundCheckOrigin.position, -transform.up, groundCheckDistance, whatIsGround);
        isFacingWall = Physics2D.Raycast(transform.position, transform.right * (facingRight ? 1 : -1), wallCheckDistance, whatIsGround);
    }

    void OnDrawGizmos()
    {
        // Ground Ahead Check Ray
        Gizmos.DrawLine(groundCheckOrigin.position, new Vector2(groundCheckOrigin.position.x, groundCheckOrigin.position.y - groundCheckDistance));
        // Ground Below Check Ray
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        // Wall Check Ray
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + wallCheckDistance * (facingRight ? 1 : -1), transform.position.y));
    }
}
