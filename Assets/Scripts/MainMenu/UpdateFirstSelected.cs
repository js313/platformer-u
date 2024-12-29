using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateFirstSelected : MonoBehaviour
{
    MainMenu mainMenu;
    [SerializeField] GameObject firstSelected;

    void Awake()
    {
        mainMenu = GetComponentInParent<MainMenu>();
    }
    void OnEnable()
    {
        mainMenu.UpdateLastSelected(firstSelected);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}
