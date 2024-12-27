using System;
using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public static event Action OnPlayerRespawn;

    [Header("Player")]
    [SerializeField] Player playerPrefab;
    [SerializeField] float respawnDelay;
    [SerializeField] Transform playerRespawnPoint;
    public Player player;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (playerRespawnPoint == null)
            playerRespawnPoint = FindFirstObjectByType<Startpoint>().transform;
        if (player == null)
        {
            player = FindFirstObjectByType<Player>();
            OnPlayerRespawn?.Invoke();
        }
    }

    public void RespawnPlayer() => StartCoroutine(RespawnCoroutine());

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        player = Instantiate(playerPrefab, playerRespawnPoint.position, Quaternion.identity);
        OnPlayerRespawn?.Invoke();
    }

    public void UpdatePlayerRespawnPoint(Transform newPlayerRespawnPoint) => playerRespawnPoint = newPlayerRespawnPoint;
}
