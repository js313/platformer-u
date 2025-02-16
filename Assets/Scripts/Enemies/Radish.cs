using UnityEngine;

enum STATE
{
    Flying,
    Falling,
    Walking
}

public class Radish : Enemy
{
    [SerializeField] float bobSpeed = 1;
    [SerializeField] float minWaitTimeBeforeFlying = 3;
    float hitTime = float.PositiveInfinity;

    STATE currentState = STATE.Flying;
    float flyingPositionX;
    float flyingHeight;

    protected override void Start()
    {
        base.Start();

        flyingPositionX = transform.position.x;
        flyingHeight = HeightCheck();
    }

    float HeightCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, float.PositiveInfinity, whatIsGround);
        return hit.distance;
    }

    protected override void Update()
    {
        base.Update();

        if (currentState == STATE.Walking)
        {
            HandleAnimation();
            isIdle = false;
            HandleFlip();
            HandleMovement();

            if (Time.time - hitTime > minWaitTimeBeforeFlying && Mathf.Abs(transform.position.x - flyingPositionX) < 0.2f)
            {
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("onGround", false);
                currentState = STATE.Flying;
                hitTime = float.PositiveInfinity;
            }
        }
        else if (currentState == STATE.Flying)
        {
            rb.gravityScale = 0;
            float posY = transform.position.y;
            if (transform.position.y - flyingHeight > 0.3f && bobSpeed > 0) bobSpeed *= -1;
            else if (transform.position.y - flyingHeight < -0.3f && bobSpeed < 0) bobSpeed *= -1;
            posY += bobSpeed * Time.deltaTime;
            transform.position = new(transform.position.x, posY, transform.position.z);
        }
        else
        {
            rb.gravityScale = 3;
            if (isOnGround)
            {
                currentState = STATE.Walking;
                anim.SetBool("onGround", true);
            }
        }
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

    protected override void HandleDeath()
    {
        if (currentState == STATE.Flying)
        {
            hitTime = Time.time;
            currentState = STATE.Falling;
        }
        else
        {
            rb.gravityScale = 1;
            base.HandleDeath();
        }
    }
}
