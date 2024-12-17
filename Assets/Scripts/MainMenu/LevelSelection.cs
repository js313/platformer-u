using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] LevelButton LevelButtonPrefab;
    [SerializeField] Transform buttonParent;

    void Start()
    {
        CreateButton();
    }

    public void CreateButton()
    {
        int levelAmount = SceneManager.sceneCountInBuildSettings - 1;   // To exclude End Scene

        for (int i = 1; i < levelAmount; i++)   // i = 1, to exclude first(Menu Scene)
        {
            LevelButton levelButtonInstance = Instantiate(LevelButtonPrefab, buttonParent);
            levelButtonInstance.SetupButton(i);
        }
    }
}
