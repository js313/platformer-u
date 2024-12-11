using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [Header("Knockback")]
    [SerializeField] float knockBackDuration = 0.6f;
    [SerializeField] Vector2 knockBackSpeed = new(5, 7);

    void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.KnockBack(transform, knockBackSpeed, knockBackDuration);
        }
    }
}
