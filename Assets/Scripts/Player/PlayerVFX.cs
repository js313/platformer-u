using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVFX : MonoBehaviour
{
    Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void RespawnFinished()
    {
        player.RespawnFinished(true);
    }
}
