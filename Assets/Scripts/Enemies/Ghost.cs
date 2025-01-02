using NUnit.Framework;
using UnityEngine;

public class Ghost : Enemy
{
    SpriteRenderer sr;

    Collider2D[] colliders;

    [SerializeField] float activeDuration;
    [SerializeField] float xMinDist;
    [SerializeField] float yMaxDist;
    Transform target;

    float activeTime;

    bool isChasing;

    protected override void Awake()
    {
        base.Awake();

        sr = GetComponent<SpriteRenderer>();

        colliders = GetComponentsInChildren<Collider2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (isDead) return;


        if (activeTime > 0) activeTime -= Time.deltaTime;
        if (idleTime > 0) idleTime -= Time.deltaTime;

        if (!isChasing && idleTime <= 0)
        {
            StartChase();
        }
        else if (isChasing && activeTime <= 0)
        {
            EndChase();
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (!target) return;

        HandleFlip();

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
    }

    void StartChase()
    {
        // List<Player> players = PlayerManager.instance.GetPlayerList();    // After implementing local coop with multiple players
        if (PlayerManager.instance.player == null)
        {
            EndChase();
            return;
        }
        target = PlayerManager.instance.player.transform;

        float yPosition = Random.Range(-yMaxDist, yMaxDist);
        float xPosition = Random.Range(-1, 2) * xMinDist;

        transform.position = new Vector3(target.position.x + xPosition, target.position.y + yPosition, transform.position.z);

        HandleFlip();

        activeTime = activeDuration;
        isChasing = true;
        anim.SetTrigger("appear");
    }

    private void HandleFlip()
    {
        if (target != null && Mathf.Sign(target.position.x - transform.position.x) != (facingRight ? 1 : -1)) Flip();
    }

    void EndChase()
    {
        idleTime = idleDuration;
        isChasing = false;
        anim.SetTrigger("disappear");
    }

    void ToggleColliders(bool enable)
    {
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = enable;
        }
    }

    public void MakeInvisible()
    {
        sr.color = Color.clear;
        ToggleColliders(false);
    }

    public void MakeVisible()
    {
        sr.color = Color.white;
        ToggleColliders(true);
    }
}
