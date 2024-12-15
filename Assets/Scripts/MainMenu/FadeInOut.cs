using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField] Image fadeImage;

    public void Fade(float startAlpha, float endAlpha, float fadeDuration, System.Action onComplete)
    {
        StartCoroutine(FadeCoroutine(startAlpha, endAlpha, fadeDuration, onComplete));
    }

    IEnumerator FadeCoroutine(float startAlpha, float endAlpha, float fadeDuration, System.Action onComplete)
    {
        float time = 0;
        Color startColor = new(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, startAlpha);
        Color endColor = new(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, endAlpha);
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, time / fadeDuration);
            yield return null;
        }
        fadeImage.color = endColor;
        onComplete?.Invoke();
    }
}
