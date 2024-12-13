using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailShell : Enemy
{
    int hitCount = 0;

    protected override void Awake()
    {
        base.Awake();

        isIdle = true;
    }

    protected override void Update()
    {
        base.Update();

        HandleAnimation();
        HandleFlip();
        HandleMovement();
    }

    protected override void HandleDeath()
    {
        hitCount++;
        if (hitCount > 1)
            base.HandleDeath();
        else
            isIdle = false;
    }

    void HandleFlip()
    {
        if (isOnGround && isFacingWall)
        {
            Flip();
        }
    }

    void HandleAnimation()
    {
        if (isFacingWall)
        {
            anim.SetTrigger("wallHit");
        }
    }

    void HandleMovement()
    {
        if (isDead || isIdle) return;

        rb.velocity = new Vector2(moveSpeed * (facingRight ? 1 : -1), rb.velocity.y);
    }
}
