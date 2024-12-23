using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelNumberText;
    [SerializeField] TextMeshProUGUI bestTimeText;
    [SerializeField] TextMeshProUGUI fruitsInfoText;
    int levelIndex;

    public void SetupButton(int levelIndex, int fruitsCollected, int totalFruits, float bestTime)
    {
        this.levelIndex = levelIndex;

        levelNumberText.text = "Level " + levelIndex;
        bestTimeText.text = "Best Time: " + (bestTime == 0 ? "- s" : bestTime.ToString("00") + "s");
        fruitsInfoText.text = "Fruits: " + (totalFruits == 0 ? "0/?" : fruitsCollected.ToString() + "/" + totalFruits.ToString());
    }

    public void LoadLevel()
    {
        AudioManager.instance.PlaySfx(4);

        SceneManager.LoadScene("Level_" + levelIndex);
    }
}
