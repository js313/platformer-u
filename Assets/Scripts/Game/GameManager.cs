using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Fade")]
    [SerializeField] float fadeDuration = 1.0f;

    [SerializeField] int currentLevelIndex;

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
        GameUI.instance.FadeIn(fadeDuration, null);
        totalFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None).Length;
    }

    private void LoadNextScene() => SceneManager.LoadScene("Level_" + (currentLevelIndex + 1));
    private void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");

    public void RespawnPlayer() => StartCoroutine(RespawnCoroutine());

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        player = Instantiate(playerPrefab, playerRespawnPoint.position, Quaternion.identity);
    }

    public void UpdatePlayerRespawnPoint(Transform newPlayerRespawnPoint) => playerRespawnPoint = newPlayerRespawnPoint;

    public void FruitCollected() => fruitsCollected++;

    public bool GetRandomizeFruits() => randomizeFruits;

    public void LevelFinished()
    {
        if (currentLevelIndex < SceneManager.sceneCountInBuildSettings - 2) // Excluding Main menu and End Scene
            GameUI.instance.FadeOut(fadeDuration, LoadNextScene);
        else
            GameUI.instance.FadeOut(fadeDuration, LoadTheEndScene);
    }
}
