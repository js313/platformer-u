using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Player")]
    [SerializeField] Player playerPrefab;
    [SerializeField] float respawnDelay;
    public Player player;

    [SerializeField] Transform playerRespawnPoint;

    [Header("Fruits")]
    public int totalFruits;
    public int fruitsCollected = 0;
    public bool randomizeFruits = false;

    [Header("Checkpoints")]
    public bool canCheckpointsBeReActivated = false;

    [Header("Fade")]
    [SerializeField] float fadeDuration = 0.75f;

    [Header("Managers")]
    [SerializeField] AudioManager audioManager;

    [SerializeField] int currentLevelIndex;
    int nextLevelIndex;

    float levelTimer = 0;
    GameUI gameUI;
    public bool isGamePaused { get; private set; }

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
        nextLevelIndex = currentLevelIndex + 1;
    }

    private void Start()
    {
        gameUI = GameUI.instance;
        gameUI.FadeIn(fadeDuration, null);
        totalFruits = FindObjectsByType<Fruit>(FindObjectsSortMode.None).Length;
        gameUI.UpdateFruitCounter(0, totalFruits);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "TotalFruits", totalFruits);
        levelTimer = 0;

        //if (playerRespawnPoint == null)
        //    playerRespawnPoint = FindFirstObjectByType<Startpoint>().transform;
        //if (player == null)
        //    player = FindFirstObjectByType<Player>();

        CreateManagers();
    }

    void Update()
    {
        levelTimer += Time.deltaTime;

        gameUI.UpdateTimer(levelTimer);
    }

    void CreateManagers()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManager);
        }
    }

    public void RestartLevel()
    {
        gameUI.FadeOut(fadeDuration, LoadCurrentScene);
    }

    private void LoadCurrentScene() => SceneManager.LoadScene("Level_" + currentLevelIndex);
    private void LoadNextScene() => SceneManager.LoadScene("Level_" + (nextLevelIndex));
    private void LoadTheEndScene() => SceneManager.LoadScene("TheEnd");

    public void PauseUnpauseGame(bool isPaused) => isGamePaused = isPaused;

    public void RespawnPlayer() => StartCoroutine(RespawnCoroutine());

    IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        player = Instantiate(playerPrefab, playerRespawnPoint.position, Quaternion.identity);
    }

    public void UpdatePlayerRespawnPoint(Transform newPlayerRespawnPoint) => playerRespawnPoint = newPlayerRespawnPoint;

    public void FruitCollected()
    {
        fruitsCollected++;
        gameUI.UpdateFruitCounter(fruitsCollected, totalFruits);
    }

    public void FruitDropped()
    {
        fruitsCollected--;
        gameUI.UpdateFruitCounter(fruitsCollected, totalFruits);
    }

    public bool GetRandomizeFruits() => randomizeFruits;

    public void LevelFinished()
    {
        SavePlayerPrefs();
        if (currentLevelIndex < SceneManager.sceneCountInBuildSettings - 2) // Excluding Main menu and End Scene
        {
            gameUI.FadeOut(fadeDuration, LoadNextScene);
        }
        else
        {
            PlayerPrefs.SetInt("ContinueLevelNumber", 0);

            gameUI.FadeOut(fadeDuration, LoadTheEndScene);
        }
    }

    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("Level" + nextLevelIndex + "Unlocked", 1);
        PlayerPrefs.SetInt("ContinueLevelNumber", nextLevelIndex);
        int fruitsInBank = PlayerPrefs.GetInt("FruitsInBank");
        int previousMaxFruitsCollected = PlayerPrefs.GetInt("Level" + currentLevelIndex + "FruitsCollected");
        float currentCompletionTime = levelTimer;
        float previousCompletionTime = PlayerPrefs.GetFloat("Level" + currentLevelIndex + "BestCompletionTime");
        previousCompletionTime = previousCompletionTime == 0 ? Mathf.Infinity : previousCompletionTime;

        PlayerPrefs.SetInt("FruitsInBank", fruitsInBank + fruitsCollected);
        PlayerPrefs.SetInt("Level" + currentLevelIndex + "FruitsCollected", Math.Max(fruitsCollected, previousMaxFruitsCollected));
        PlayerPrefs.SetFloat("Level" + currentLevelIndex + "BestCompletionTime", Math.Min(previousCompletionTime, currentCompletionTime));
    }
}
