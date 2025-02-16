using UnityEngine;

public class BlueBird : Enemy
{
    [SerializeField] float flyDistance;
    [SerializeField] float flySpeed;
    [SerializeField] float declineSpeed;
    Vector3 rightPoint, leftPoint;

    protected override void Start()
    {
        base.Start();

        rightPoint = transform.position + Vector3.right * flyDistance / 2;
        leftPoint = transform.position + Vector3.left * flyDistance / 2;
    }

    protected override void Update()
    {
        base.Update();
     
        if(isDead) return;

        transform.position -= flySpeed * Time.deltaTime * transform.right;
        transform.position -= declineSpeed * Time.deltaTime * transform.up;

        if ((Mathf.Abs(transform.position.x - leftPoint.x) < 0.1f && !facingRight) || (Mathf.Abs(transform.position.x - rightPoint.x) < 0.1f && facingRight))
        {
            Flip();
        }
    }

    public void FlapUp()
    {
        transform.position = new Vector3(transform.position.x, leftPoint.y, transform.position.z);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(leftPoint, 1);
        Gizmos.DrawWireSphere(rightPoint, 1);
    }
}
