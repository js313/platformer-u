using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Enemy
{
    [Header("Player Detection")]
    [SerializeField] float playerDetectionDistance;
    [SerializeField] float changeDirectionDelay = 0.2f;
    bool isPlayerInSight = false;
    bool waitingForFlip = false;

    protected override void Update()
    {
        base.Update();

        HandleAnimation();
        isIdle = false;
        HandleFlip();
        HandleMovement();
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

            if (isAttacking &&
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
        if (Mathf.Sign(player.transform.position.x - transform.position.x) != (facingRight ? 1 : -1)) Flip();
        waitingForFlip = false;
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

    void HandleAnimation() => anim.SetFloat("xVelocity", isAttacking ? rb.velocity.x : 0);

    void HandleMovement()
    {
        if (isDead) return;

        if (!isAttacking)
        {
            rb.velocity = Vector3.zero;
            return;
        }

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

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        // Player Detection Ray
        Gizmos.DrawLine(transform.position, transform.position + (facingRight ? 1 : -1) * playerDetectionDistance * Vector3.right);
    }
}
