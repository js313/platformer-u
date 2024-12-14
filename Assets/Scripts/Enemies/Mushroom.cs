using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Enemy
{
    protected override void Update()
    {
        base.Update();

        HandleAnimation();
        isIdle = false;
        HandleFlip();
        HandleMovement();
    }

    void HandleFlip()
    {
        if (isOnGround && (!isGroundAhead || isFacingWall))
        {
            Flip();
            idleTime = idleDuration;
            isIdle = true;
        }
    }

    void HandleAnimation() => anim.SetFloat("xVelocity", rb.linearVelocity.x);

    void HandleMovement()
    {
        if (isDead) return;

        if (!isOnGround)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
        if (idleTime > 0)
        {
            rb.linearVelocity = Vector3.zero;
            idleTime -= Time.deltaTime;
            return;
        }
        rb.linearVelocity = new Vector2(moveSpeed * (facingRight ? 1 : -1), rb.linearVelocity.y);
    }
}
