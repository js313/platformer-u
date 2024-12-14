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
