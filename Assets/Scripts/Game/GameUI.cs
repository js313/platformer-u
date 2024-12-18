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

    void Awake()
    {
        if (instance == null) instance = this;
        fadeEffect = GetComponentInChildren<FadeInOut>();
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
