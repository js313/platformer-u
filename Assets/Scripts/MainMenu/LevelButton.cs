using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumberText;
    int levelIndex;

    public void SetupButton(int levelIndex)
    {
        this.levelIndex = levelIndex;

        levelNumberText.text = "Level " + levelIndex;
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("Level_" + levelIndex);
    }
}
