using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string sceneName;
    int continueSceneIndex;

    Button[] buttons;

    [SerializeField] Button continueButton;

    [SerializeField] float fadeDuration = 0.5f;
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
        continueSceneIndex = PlayerPrefs.GetInt("ContinueLevelNumber");
        if (continueSceneIndex == 0) continueButton.gameObject.SetActive(false);
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

    public void LoadLastLevel() => SceneManager.LoadScene("Level_" + continueSceneIndex);

    void EnableMenuButtons()
    {
        foreach (Button button in buttons)
        {
            if (button == continueButton)
            {
                if (continueSceneIndex == 0)
                {
                    continueButton.gameObject.SetActive(false);
                    continueButton.enabled = false;
                    continue;
                }
            }
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
