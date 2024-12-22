using UnityEngine;

public enum DifficultyType { Easy, Normal, Hard }

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager instance;

    public DifficultyType difficulty { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(DifficultyType type)
    {
        PlayerPrefs.SetInt("LastDifficulty", (int)type);
        difficulty = type;
    }
}
