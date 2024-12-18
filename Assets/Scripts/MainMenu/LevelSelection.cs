using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] LevelButton LevelButtonPrefab;
    [SerializeField] Transform buttonParent;

    void Start()
    {
        PlayerPrefs.SetInt("Level1Unlocked", 1);
        CreateButton();
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
