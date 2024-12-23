using UnityEngine;

public class MenuCharacter : MonoBehaviour
{
    Animator anim;

    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float speed;
    Vector3 pointAPos, pointBPos;

    private void Awake()
    {
        pointAPos = pointA.position;
        pointBPos = pointB.position;
        transform.position = pointAPos;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(transform.position != pointBPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointBPos, Time.deltaTime * speed);
            anim.SetBool("isMoving", true);
            return;
        }
        anim.SetBool("isMoving", false);
    }
}
