using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    FadeInOut fadeEffect;

    [SerializeField] TextMeshProUGUI fruitCounter;
    [SerializeField] TextMeshProUGUI timer;

    [SerializeField] GameObject pauseUI;
    bool isGamePaused = false;

    void Awake()
    {
        if (instance == null) instance = this;
        fadeEffect = GetComponentInChildren<FadeInOut>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGamePaused = !isGamePaused;
            if (isGamePaused) PauseGame();
            else UnPauseGame();
        }
    }

    void PauseGame()
    {
        isGamePaused = true;
        GameManager.instance.PauseUnpauseGame(isGamePaused);
        Time.timeScale = 0.0f;
        pauseUI.SetActive(isGamePaused);
    }

    void UnPauseGame()
    {
        isGamePaused = false;
        GameManager.instance.PauseUnpauseGame(isGamePaused);
        Time.timeScale = 1.0f;
        pauseUI.SetActive(isGamePaused);
    }

    public void ResumeGame()
    {
        UnPauseGame();
    }

    public void MainMenu()
    {
        UnPauseGame();
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateFruitCounter(int fruitsCollected, int totalFruits)
    {
        fruitCounter.text = fruitsCollected.ToString() + "/" + totalFruits.ToString();
    }

    public void UpdateTimer(float time)
    {
        timer.text = time.ToString("00") + " s";
    }

    public void FadeIn(float fadeDuration, System.Action callback)
    {
        fadeEffect.Fade(1, 0, fadeDuration, callback);
    }

    public void FadeOut(float fadeDuration, System.Action callback)
    {
        fadeEffect.Fade(0, 1, fadeDuration, callback);
    }
}
