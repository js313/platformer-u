using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrap : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D[] cds;
    Animator anim;

    bool canMove = true;

    [Header("Hover")]
    [SerializeField] float hoverSpeed;
    [SerializeField] float hoverDistance;
    [SerializeField] float randomizeMoveTime;
    float startTimeOffset = 0;
    Vector2 minMaxY;

    [Header("Fall")]
    [SerializeField] float fallWaitTime = 1;
    [SerializeField] float destroyAfterFallTime = 5;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cds = GetComponents<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        startTimeOffset = Random.Range(0, randomizeMoveTime);
        minMaxY = new Vector2(transform.position.y - hoverDistance, transform.position.y + hoverDistance);
    }

    void Update()
    {
        if (!canMove) return;

        transform.position = Vector2.Lerp(new Vector2(transform.position.x, minMaxY.x),
            new Vector2(transform.position.x, minMaxY.y),
            (Mathf.Sin((Time.time + startTimeOffset) * hoverSpeed) + 1) / 2);
    }

    void Fall()
    {
        anim.SetTrigger("stop");
        foreach (Collider2D cd in cds) { cd.enabled = false; }
        rb.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyAfterFallTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            canMove = false;
            anim.SetTrigger("wobble");
            Invoke(nameof(Fall), fallWaitTime);
        }
    }
}
