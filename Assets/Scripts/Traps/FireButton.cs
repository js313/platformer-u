using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour
{
    Fire fire;
    Animator anim;

    void Awake()
    {
        fire = GetComponentInParent<Fire>();
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            fire.ToggleFire();
            anim.SetTrigger("toggle");
        }
    }
}
