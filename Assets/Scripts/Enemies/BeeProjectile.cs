using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] string groundLayerName;
    [SerializeField] string playerLayerName;
    [SerializeField] float collisionDestroyAfter;
    [SerializeField] float automaticallyDestroyAfter;
    public Vector3 direction;

    readonly Queue<Vector3> wayPoints = new();
    [SerializeField] float sqrDistThreshold = 50f;
    [SerializeField] float addWaypointEvery = 2.0f;

    void Start()
    {
        if (PlayerManager.instance.player != null)
        {
            wayPoints.Enqueue(PlayerManager.instance.player.transform.position);
            direction = (wayPoints.Peek() - transform.position).normalized;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        Destroy(gameObject, automaticallyDestroyAfter);

        StartCoroutine(AddPlayerLocationToWaypoints());
    }

    IEnumerator AddPlayerLocationToWaypoints()
    {
        while (true)
        {
            if (PlayerManager.instance.player != null)
            {
                wayPoints.Enqueue(PlayerManager.instance.player.transform.position);
            }

            yield return new WaitForSeconds(addWaypointEvery);
        }
    }

    void Update()
    {
        if (wayPoints.Count == 0) return;

        direction = (wayPoints.Peek() - transform.position).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) + 90;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.position += speed * Time.deltaTime * direction;
        if (Vector2.SqrMagnitude(transform.position - wayPoints.Peek()) <= sqrDistThreshold)
        {
            wayPoints.Dequeue();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName) ||
            collision.gameObject.layer == LayerMask.NameToLayer(playerLayerName))
        {
            Destroy(gameObject, collisionDestroyAfter);
        }
    }
}
