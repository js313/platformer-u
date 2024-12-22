using UnityEngine;

public class DifficultySelection : MonoBehaviour
{
    DifficultyManager difficultyManager;

    void Start()
    {
        difficultyManager = DifficultyManager.instance;
    }

    public void SetDifficultyToEasy() => difficultyManager.SetDifficulty(DifficultyType.Easy);
    public void SetDifficultyToNormal() => difficultyManager.SetDifficulty(DifficultyType.Normal);
    public void SetDifficultyToHard() => difficultyManager.SetDifficulty(DifficultyType.Hard);
}
