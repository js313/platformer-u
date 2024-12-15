using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    FadeInOut fadeEffect;
    [SerializeField] float fadeDuration = 1.0f;

    [SerializeField] RectTransform credits;
    [SerializeField] float scrollSpeed;
    [SerializeField] float creditOffScreenPoint;
    bool creditsSkipped = false;

    void Awake()
    {
        fadeEffect = GetComponentInChildren<FadeInOut>();
    }

    void Start()
    {
        fadeEffect.Fade(1, 0, fadeDuration, null);
    }

    void Update()
    {
        credits.anchoredPosition += scrollSpeed * Time.deltaTime * Vector2.up;
        if (credits.anchoredPosition.y >= creditOffScreenPoint) EndCredit();
    }

    void EndCredit()
    {
        fadeEffect.Fade(0, 1, fadeDuration, LoadMainMenu);
    }

    void LoadMainMenu() => SceneManager.LoadScene("MainMenu");

    public void SkipCredits()
    {
        if (creditsSkipped)
        {
            EndCredit();
        }
        else
        {
            creditsSkipped = true;
            scrollSpeed *= 10;
        }
    }
}
