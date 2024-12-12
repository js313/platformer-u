using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : Enemy
{
    [Header("Attack")]
    [SerializeField] Projectile projectile;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] float attackCooldown;
    float nextAttackIn = 0;

    protected override void Update()
    {
        base.Update();

        HandleAnimation();
        isIdle = false;
        HandleFlip();
        HandleAttack();
        HandleMovement();
    }

    protected void HandleAttack()
    {
        nextAttackIn -= Time.deltaTime;
        if (isPlayerInSight && nextAttackIn <= 0)
        {
            isAttacking = true;
            anim.SetTrigger("attack");
            nextAttackIn = attackCooldown;
            idleTime = idleDuration;
        }
        if(!isPlayerInSight) idleTime -= Time.deltaTime;
        if (idleTime <= 0) isAttacking = false;
    }

    public void ShootProjectile()
    {
        Projectile projectileInstance = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        projectileInstance.transform.parent = transform;
        projectileInstance.direction = facingRight ? Vector2.right : -Vector2.right;
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

        if (!isOnGround || isAttacking)
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
