using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    public Player player;
    [SerializeField] Player playerPrefab;
    [SerializeField] float respawnDelay;

    [SerializeField] Transform playerRespawnPoint;

    [Header("Fruits")]
    public int totalFruits;
    public int fruitsCollected = 0;
    public bool randomizeFruits = false;

    [Header("Checkpoints")]
    public bool canCheckpointsBeReActivated = false;

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

    private void Start()
    {
        totalFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None).Length;
    }

    public void RespawnPlayer() => StartCoroutine(RespawnCoroutine());

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        player = Instantiate(playerPrefab, playerRespawnPoint.position, Quaternion.identity);
    }

    public void UpdatePlayerRespawnPoint(Transform newPlayerRespawnPoint) => playerRespawnPoint = newPlayerRespawnPoint;

    public void FruitCollected() => fruitsCollected++;

    public bool GetRandomizeFruits() => randomizeFruits;
}
