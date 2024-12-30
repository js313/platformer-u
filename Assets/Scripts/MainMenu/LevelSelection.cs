using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    MainMenu menu;
    [SerializeField] GameObject firstSelected;
    DefaultInputActions inputActions;

    [SerializeField] LevelButton LevelButtonPrefab;
    [SerializeField] Transform buttonParent;

    private void Awake()
    {
        menu = GetComponentInParent<MainMenu>();
        inputActions = new DefaultInputActions();
        PlayerPrefs.SetInt("Level1Unlocked", 1);
        CreateButton();
    }
    
    private void OnEnable()
    {
        menu.UpdateLastSelected(firstSelected);
        if(firstSelected != null) EventSystem.current.SetSelectedGameObject(firstSelected);
        else EventSystem.current.SetSelectedGameObject(buttonParent.GetChild(0).gameObject);
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void CreateButton()
    {
        int levelAmount = SceneManager.sceneCountInBuildSettings - 1;   // To exclude End Scene

        for (int i = 1; i < levelAmount; i++)   // i = 1, to exclude first(Menu Scene)
        {
            if (PlayerPrefs.GetInt("Level" + i + "Unlocked") == 1)
            {
                int levelFruitsCollected = PlayerPrefs.GetInt("Level" + i + "FruitsCollected");
                int levelTotalFruits = PlayerPrefs.GetInt("Level" + i + "TotalFruits");
                float levelBestTime = PlayerPrefs.GetFloat("Level" + i + "BestCompletionTime");
                LevelButton levelButtonInstance = Instantiate(LevelButtonPrefab, buttonParent);

                levelButtonInstance.SetupButton(i,levelFruitsCollected, levelTotalFruits, levelBestTime);
            }
        }
    }
}
