using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Animator anim;
    bool active = false;

    bool canBeReActivated;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        canBeReActivated = GameManager.instance.canCheckpointsBeReActivated;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (active && !canBeReActivated) return;

        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            ActivateCheckpoint();
        }
    }

    void ActivateCheckpoint()
    {
        active = true;
        anim.SetTrigger("activate");
        PlayerManager.instance.UpdatePlayerRespawnPoint(transform);
    }
}
