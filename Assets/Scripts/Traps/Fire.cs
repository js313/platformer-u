using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    Collider2D cd;
    Animator anim;

    bool isActive = true;

    [SerializeField] float turnBackOnTime = 3;

    void Awake()
    {
        cd = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        anim.SetBool("active", true);
    }

    public void TurnOffFire()
    {
        if (!isActive) return;

        cd.enabled = false;
        isActive = false;
        anim.SetBool("active", false);

        Invoke(nameof(TurnOnFire), turnBackOnTime);
    }

    public void TurnOnFire()
    {
        if (isActive) return;

        cd.enabled = true;
        isActive = true;
        anim.SetBool("active", true);
    }
}
