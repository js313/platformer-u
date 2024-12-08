using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Animator anim;
    Collider2D cd;

    [Header("Push")]
    [SerializeField] float pushForce;
    [SerializeField] float pushDuration;

    [Header("Animation")]
    [SerializeField] float rotationSpeed;
    [SerializeField] int rotationDirection = 1;
    [SerializeField] float enableWaitTime;
    [SerializeField] float animationTime = 0.5f;
    [SerializeField] float targetScale = 1.5f;

    bool isActive = true;
    bool isCoroutineRunning = false;

    void Awake()
    {
        cd = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isActive) return;
        transform.Rotate(0, 0, rotationSpeed * rotationDirection * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            anim.SetBool("activate", true);
            player.Push(transform.up, pushForce, pushDuration);
        }
    }

    IEnumerator AnimateAppearance()
    {
        float percent = 0;
        Vector3 start = isActive ? transform.localScale : Vector3.one * targetScale;
        Vector3 end = !isActive ? Vector3.zero : Vector3.one * targetScale;
        while (percent <= 1)
        {
            transform.localScale = Vector3.Lerp(start, end, percent);
            percent += Time.deltaTime / animationTime;
            yield return null;
        }
        transform.localScale = end;
    }

    void DisableCollision()
    {
        cd.enabled = false;
        isActive = false;
        StartCoroutine(AnimateAppearance());
    }

    public void DisableArrow()
    {
        if (isCoroutineRunning) return;
        StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine()
    {
        isCoroutineRunning = true;
        // Disable
        DisableCollision();
        yield return new WaitForSeconds(animationTime);

        // Stay
        yield return new WaitForSeconds(enableWaitTime);

        // Enable
        isActive = true;
        anim.SetBool("activate", false);
        StartCoroutine(AnimateAppearance());
        yield return new WaitForSeconds(animationTime);
        cd.enabled = true;
        isCoroutineRunning = false;
    }
}
