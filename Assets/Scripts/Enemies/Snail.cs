using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : Enemy
{
    [Header("Shell")]
    [SerializeField] SnailShell shell;

    bool isHitAnimFinished = false;

    protected override void Update()
    {
        base.Update();

        HandleAnimation();
        isIdle = false;
        HandleFlip();
        HandleMovement();
    }

    protected override void HandleDeath()
    {
        if (!isHitAnimFinished) return;

        SnailShell shellInstance = Instantiate(shell, transform.position, Quaternion.identity);
        shellInstance.transform.parent = transform.parent;
        shellInstance.transform.rotation = transform.rotation;
        base.HandleDeath();
    }

    public void CallHandleDeath()   // Through animation trigger
    {
        isHitAnimFinished = true;
        HandleDeath();
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
