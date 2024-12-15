using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public static GameUI instance;

    FadeInOut fadeEffect;

    void Awake()
    {
        if (instance == null) instance = this;
        fadeEffect = GetComponentInChildren<FadeInOut>();
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
