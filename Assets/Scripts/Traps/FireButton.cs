using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour
{
    Fire fire;
    Animator anim;

    bool isFireActive = true;

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
            if (isFireActive)
            {
                fire.TurnOffFire();
            }
            else
            {
                fire.TurnOnFire();
            }
            isFireActive = !isFireActive;
            anim.SetTrigger("toggle");
        }
    }
}
