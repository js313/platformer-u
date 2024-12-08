using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    Animator anim;
    Vector2[] waypoints;
    SpriteRenderer sprite;

    [SerializeField]
    float speed;
    int nextWaypoint;
    int moveDirection = 1;

    [SerializeField]
    float pauseTime = 0.5f;
    bool paused = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        Transform[] waypointTransforms = GetComponentsInChildren<Transform>();
        waypointTransforms = System.Array.FindAll(waypointTransforms, t => t != transform);

        waypoints = new Vector2[waypointTransforms.Length];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypointTransforms[i].position;
            Destroy(waypointTransforms[i].gameObject);
        }
        transform.position = waypoints[0];
        nextWaypoint = 1;
    }

    void Start()
    {
        anim.SetBool("active", true);
    }

    void Update()
    {
        if (paused) return;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[nextWaypoint], speed * Time.deltaTime);

        if (Vector2.SqrMagnitude(new Vector2(transform.position.x, transform.position.y) - waypoints[nextWaypoint]) == 0)
        {
            if (nextWaypoint == 0 || nextWaypoint == waypoints.Length - 1)
            {
                moveDirection = -moveDirection;
                sprite.flipX = moveDirection > 0;
                StartCoroutine(PauseMovement());
            }
            nextWaypoint += moveDirection;
        }
    }

    IEnumerator PauseMovement()
    {
        paused = true;
        anim.SetBool("active", false);
        yield return new WaitForSeconds(pauseTime);
        anim.SetBool("active", true);
        paused = false;
    }
}
