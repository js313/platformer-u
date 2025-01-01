using System;
using UnityEngine;

public class Bat : Enemy
{
    [SerializeField] float aggroRange = 7;
    [SerializeField] float chaseDuration = 1;
    [SerializeField] float attackSpeed = 10;
    float defaultSpeed;
    float chaseTime;

    Vector3 originalPosition;
    Vector3 destination;

    bool canDetectPlayer = true;
    bool canMove = false;

    Collider2D target;

    protected override void Awake()
    {
        base.Awake();

        originalPosition = transform.position;
        defaultSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead)
        {
            moveSpeed = 0;
            return;
        }

        if (idleTime < 0)
            canDetectPlayer = true;
        else
            idleTime -= Time.deltaTime;

        if (chaseTime >= 0)
            chaseTime -= Time.deltaTime;

        HandleTargetDetection();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!canMove) return;

        if (Mathf.Sign(destination.x - transform.position.x) != (facingRight ? 1 : -1)) Flip();
        transform.position = Vector2.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        if (chaseTime > 0 && target)    // Basically following but not attacking(still would do damage if player collided)
            destination = target.transform.position;
        else
            moveSpeed = attackSpeed;

        if (Vector2.SqrMagnitude(destination - transform.position) < 0.1f)
        {
            if (destination == originalPosition)
            {
                idleTime = idleDuration;
                canDetectPlayer = false;
                target = null;
                canMove = false;
                anim.SetBool("isMoving", false);
                moveSpeed = defaultSpeed;
            }
            else
            {
                destination = originalPosition;
            }
        }
    }

    void HandleTargetDetection()
    {
        if (!target && canDetectPlayer)
        {
            target = Physics2D.OverlapCircle(transform.position, aggroRange, whatIsPlayer);

            if (target)
            {
                chaseTime = chaseDuration;
                destination = target.transform.position;
                canDetectPlayer = false;
                anim.SetBool("isMoving", true);
            }
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}
