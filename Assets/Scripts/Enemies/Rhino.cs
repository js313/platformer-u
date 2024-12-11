using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhino : Enemy
{
    [Header("Acceleration")]
    [SerializeField] float acceleration = 1.5f;
    [SerializeField] float maxSpeed = 10f;
    float currentMoveSpeed;

    [Header("Wall Collision")]
    [SerializeField] Vector2 impactForce;
    bool wallCollision = false;

    protected override void Awake()
    {
        base.Awake();
        currentMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        HandleAnimation();
        isIdle = false;
        HandleFlip();
        HandleWallCollision();
        HandleMovement();
    }

    private void HandleWallCollision()
    {
        if (isFacingWall && !isDead)
        {
            wallCollision = true;
            anim.SetBool("hitWall", true);
            rb.velocity = new Vector2(impactForce.x * (facingRight ? -1 : 1), impactForce.y);
        }
    }

    public void EndWallCollision()
    {
        wallCollision = false;
        isAttacking = false;
        anim.SetBool("hitWall", false);
        Flip();
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        isPlayerInSight = Physics2D.Raycast(transform.position, Vector2.right * (facingRight ? 1 : -1), playerDetectionDistance, whatIsPlayer);
    }

    protected override void HandleAttack()
    {
        base.HandleAttack();

        if (isPlayerInSight)
        {
            isAttacking = true;
            stopAttackTime = stopAttackDuration;
        }
        else
        {
            stopAttackTime -= Time.deltaTime;
            if (stopAttackTime <= 0) { isAttacking = false; }
        }
    }

    void HandleFlip()
    {
        if (isOnGround && !isGroundAhead)
        {
            Flip();
            idleTime = idleDuration;
            isIdle = true;
        }
    }

    void HandleAnimation() => anim.SetFloat("xVelocity", isAttacking ? rb.velocity.x : 0);

    void HandleMovement()
    {
        if (isDead)
        {
            rb.gravityScale = 1;
            return;
        }
        if (wallCollision) return;

        if (!isAttacking)
        {
            currentMoveSpeed = moveSpeed;
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        if (!isOnGround)
        {
            currentMoveSpeed = moveSpeed;
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        if (idleTime > 0)
        {
            currentMoveSpeed = moveSpeed;
            rb.velocity = Vector3.zero;
            idleTime -= Time.deltaTime;
            return;
        }
        currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, maxSpeed, acceleration * Time.deltaTime);
        rb.velocity = new Vector2(currentMoveSpeed * (facingRight ? 1 : -1), rb.velocity.y);
    }
}
