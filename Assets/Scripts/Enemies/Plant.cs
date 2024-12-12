using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    [Header("Attack")]
    [SerializeField] Projectile projectile;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] float attackCooldown;
    float nextAttackIn = 0;

    protected override void Update()
    {
        base.Update();

        HandleAttack();
    }

    protected void HandleAttack()
    {
        nextAttackIn -= Time.deltaTime;
        if (isPlayerInSight && nextAttackIn <= 0)
        {
            anim.SetTrigger("attack");
            nextAttackIn = attackCooldown;
        }
    }

    public void ShootProjectile()
    {
        Projectile projectileInstance = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        projectileInstance.transform.parent = transform;
        projectileInstance.direction = facingRight ? Vector2.right : -Vector2.right;
    }
}
