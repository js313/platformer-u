using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D cd;

    [Header("General")]
    [SerializeField] protected float moveSpeed = 2;
    [SerializeField] protected bool facingRight = false;
    [SerializeField] protected float idleDuration = 1.5f;
    protected bool isIdle = false;
    protected float idleTime;

    [Header("Collision")]
    [SerializeField] protected float groundCheckDistance = 1;
    [SerializeField] protected float wallCheckDistance = 1;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected Transform groundCheckOrigin;
    protected bool isOnGround = true;
    protected bool isGroundAhead = true;
    protected bool isFacingWall = false;

    [Header("Death")]
    [SerializeField] float deathEffectSpeed = 5;
    [SerializeField] float deathEffectRotation = 100;
    [SerializeField] protected DamageTrigger damageTrigger;
    [SerializeField] float destroyAfter = 10;
    int deathRotationDirection = 1;
    protected bool isDead;

    [Header("Attack")]
    [SerializeField] protected float stopAttackDuration = 0;
    protected float stopAttackTime = 0;
    protected bool isAttacking = false;
    protected Player player;

    [Header("Player Detection")]
    [SerializeField] protected float playerDetectionDistance;
    protected bool isPlayerInSight = false;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<Collider2D>();
    }

    protected void Start()
    {
        player = PlayerManager.instance.player;

        if (transform.eulerAngles.y == 180) facingRight = !facingRight;
    }

    protected virtual void Update()
    {
        HandleDeathRotation();
        if (isDead) return;
        HandleCollision();
    }

    [ContextMenu("Flip facing direction")]
    public void ChangeDefaultFacingDirection()
    {
        float newRotation = transform.rotation.eulerAngles.y == 180 ? 0 : 180;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                                            newRotation,
                                            transform.eulerAngles.z);
    }

    protected void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }

    protected virtual void HandleCollision()
    {
        isOnGround = Physics2D.Raycast(transform.position, -transform.up, groundCheckDistance, whatIsGround);
        isGroundAhead = Physics2D.Raycast(groundCheckOrigin.position, -transform.up, groundCheckDistance, whatIsGround);
        isFacingWall = Physics2D.Raycast(transform.position, Vector2.right * (facingRight ? 1 : -1), wallCheckDistance, whatIsGround);
        isPlayerInSight = Physics2D.Raycast(transform.position, Vector2.right * (facingRight ? 1 : -1), playerDetectionDistance, whatIsPlayer);
    }

    protected virtual void HandleDeath()
    {
        isDead = true;
        cd.enabled = false;
        damageTrigger.gameObject.SetActive(false);
        rb.linearVelocity = new Vector2(0, deathEffectSpeed);
        if (Random.Range(0, 1) >= 0.5f) deathRotationDirection = -1;
        Destroy(gameObject, destroyAfter);
    }

    void HandleDeathRotation()
    {
        if (!isDead) return;
        transform.Rotate(0, 0, deathEffectRotation * deathRotationDirection * Time.deltaTime);
    }

    public void Die()
    {
        anim.SetTrigger("hit");
        HandleDeath();
    }

    protected virtual void OnDrawGizmos()
    {
        // Ground Ahead Check Ray
        Gizmos.DrawLine(groundCheckOrigin.position, new Vector2(groundCheckOrigin.position.x, groundCheckOrigin.position.y - groundCheckDistance));
        // Ground Below Check Ray
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        // Wall Check Ray
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + wallCheckDistance * (facingRight ? 1 : -1), transform.position.y));
        // Player Detection Ray
        Gizmos.DrawLine(transform.position, transform.position + (facingRight ? 1 : -1) * playerDetectionDistance * Vector3.right);
    }
}
