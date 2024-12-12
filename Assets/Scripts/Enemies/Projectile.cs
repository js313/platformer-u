using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] string groundLayerName;
    [SerializeField] string playerLayerName;
    [SerializeField] float collisionDestroyAfter;
    [SerializeField] float automaticallyDestroyAfter;
    public Vector3 direction;

    void Start()
    {
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 180;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Destroy(gameObject, automaticallyDestroyAfter);
    }

    void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
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
