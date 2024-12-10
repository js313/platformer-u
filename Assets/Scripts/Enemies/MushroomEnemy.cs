using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomEnemy : Enemy
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

    void HandleAnimation() => anim.SetFloat("xVelocity", rb.velocity.x);

    void HandleMovement()
    {
        if (isDead) return;

        if (!isOnGround)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }
        if (idleTime > 0)
        {
            rb.velocity = Vector3.zero;
            idleTime -= Time.deltaTime;
            return;
        }
        rb.velocity = new Vector2(moveSpeed * (facingRight ? 1 : -1), rb.velocity.y);
    }
}
