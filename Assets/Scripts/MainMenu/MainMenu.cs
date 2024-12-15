using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string sceneName;

    Button[] buttons;

    [SerializeField] float fadeDuration = 1.5f;
    FadeInOut fadeEffect;

    [SerializeField] GameObject[] uiElements;

    void Awake()
    {
        fadeEffect = GetComponentInChildren<FadeInOut>();
        buttons = GetComponentsInChildren<Button>();
        DisableMenuButtons();
    }

    void Start()
    {
        fadeEffect.Fade(1, 0, fadeDuration, EnableMenuButtons);
    }

    public void NewGame()
    {
        fadeEffect.Fade(0, 1, fadeDuration, LoadLevelScene);
    }

    public void SwitchUI(GameObject uiElementToEnable)
    {
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }
        uiElementToEnable.SetActive(true);
    }

    void LoadLevelScene() => SceneManager.LoadScene(sceneName);

    void EnableMenuButtons()
    {
        foreach (Button button in buttons)
        {
            button.enabled = true;
        }
    }

    void DisableMenuButtons()
    {
        foreach (Button button in buttons)
        {
            button.enabled = false;
        }
    }
}
