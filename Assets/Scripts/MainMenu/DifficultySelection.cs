using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultySelection : MonoBehaviour
{
    [SerializeField] GameObject firstSelected;
    MainMenu mainMenu;
    DifficultyManager difficultyManager;

    void Awake()
    {
        mainMenu = GetComponentInParent<MainMenu>();
    }

    void Start()
    {
        difficultyManager = DifficultyManager.instance;
    }

    void OnEnable()
    {
        mainMenu.UpdateLastSelected(firstSelected);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void SetDifficultyToEasy() => difficultyManager.SetDifficulty(DifficultyType.Easy);
    public void SetDifficultyToNormal() => difficultyManager.SetDifficulty(DifficultyType.Normal);
    public void SetDifficultyToHard() => difficultyManager.SetDifficulty(DifficultyType.Hard);
}
