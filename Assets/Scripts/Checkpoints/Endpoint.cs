using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour
{
    Animator anim;
    bool active = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (active) return;

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            ActivateEndpoint();
        }
    }

    void ActivateEndpoint()
    {
        active = true;
        anim.SetTrigger("activate");
    }
}
