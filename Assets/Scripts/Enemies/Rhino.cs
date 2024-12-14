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
        HandleAttack();
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
            rb.linearVelocity = new Vector2(impactForce.x * (facingRight ? -1 : 1), impactForce.y);
        }
    }

    public void EndWallCollision()
    {
        wallCollision = false;
        isAttacking = false;
        anim.SetBool("hitWall", false);
        Flip();
    }

    protected void HandleAttack()
    {
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

    void HandleAnimation() => anim.SetFloat("xVelocity", isAttacking ? rb.linearVelocity.x : 0);

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
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        if (!isOnGround)
        {
            currentMoveSpeed = moveSpeed;
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        if (idleTime > 0)
        {
            currentMoveSpeed = moveSpeed;
            rb.linearVelocity = Vector3.zero;
            idleTime -= Time.deltaTime;
            return;
        }
        currentMoveSpeed = Mathf.MoveTowards(currentMoveSpeed, maxSpeed, acceleration * Time.deltaTime);
        rb.linearVelocity = new Vector2(currentMoveSpeed * (facingRight ? 1 : -1), rb.linearVelocity.y);
    }
}
