using UnityEngine;

public class Bee : Enemy
{
    [Header("Attack")]
    [SerializeField] BeeProjectile projectile;
    [SerializeField] Transform projectileSpawn;
    [SerializeField] float attackCooldown;
    Transform target;
    [SerializeField] float buzzSpeed;
    float nextAttackIn = 0;
    readonly Vector3[] buzzPoints = new Vector3[5];
    int nextBuzzPoint = 0;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < buzzPoints.Length; i++)
        {
            buzzPoints[i] = transform.position + new Vector3(Mathf.Cos(i * (Mathf.Deg2Rad * 75)), Mathf.Sin(i * (Mathf.Deg2Rad * 75)), transform.position.z) / 2;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (Vector3.SqrMagnitude(transform.position - buzzPoints[nextBuzzPoint]) <= 0.1f)
        {
            nextBuzzPoint = Random.Range(0, buzzPoints.Length);
        }

        Vector3 direction = (buzzPoints[nextBuzzPoint] - transform.position).normalized;
        transform.position += direction * buzzSpeed * Time.deltaTime;

        if (target == null)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, float.MaxValue, whatIsPlayer);
            target = hit.transform;
        }

        HandleAttack();
    }

    protected void HandleAttack()
    {
        nextAttackIn -= Time.deltaTime;
        if (nextAttackIn <= 0 && target != null)
        {
            anim.SetTrigger("attack");
            nextAttackIn = attackCooldown;
        }
        target = null;
    }

    public void ShootProjectile()
    {
        BeeProjectile projectileInstance = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        projectileInstance.transform.parent = transform;
    }
}
