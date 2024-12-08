using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedBall : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float pushForce;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        rb.AddForce(new Vector2(pushForce, 0), ForceMode2D.Impulse);
    }
}
