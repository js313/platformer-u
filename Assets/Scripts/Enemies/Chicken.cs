using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Enemy
{
    [Header("Flip")]
    [SerializeField] float changeDirectionDelay = 0.2f;
    bool waitingForFlip = false;

    protected override void Update()
    {
        base.Update();
        HandleAnimation();
        isIdle = false;
        HandleAttack();
        HandleFlip();
        HandleMovement();
    }

    protected void HandleAttack()
    {
        if (isPlayerInSight)
        {
            idleTime = 0;   // Works because only moves while attacking
            isAttacking = true;
            stopAttackTime = stopAttackDuration;
        }
        else
        {
            stopAttackTime -= Time.deltaTime;
            if (stopAttackTime <= 0) { isAttacking = false; }

            if (!isIdle && !isDead && isAttacking &&
                Mathf.Sign(player.transform.position.x - transform.position.x) != (facingRight ? 1 : -1) && !waitingForFlip)
            {
                StartCoroutine(ChaseFlip());
            }
        }
    }

    IEnumerator ChaseFlip()
    {
        waitingForFlip = true;
        yield return new WaitForSeconds(changeDirectionDelay);
        if (!isIdle && !isDead && Mathf.Sign(player.transform.position.x - transform.position.x) != (facingRight ? 1 : -1)) Flip();
        waitingForFlip = false;
    }

    void HandleFlip()
    {
        if (isOnGround && (!isGroundAhead || isFacingWall))
        {
            Flip();
            idleTime = idleDuration;
            isIdle = true;
            isAttacking = false;
        }
    }

    void HandleAnimation() => anim.SetFloat("xVelocity", isAttacking ? rb.linearVelocity.x : 0);

    void HandleMovement()
    {
        if (isDead) return;

        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }
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
