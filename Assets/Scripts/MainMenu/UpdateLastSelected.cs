using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateLastSelected : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    MainMenu mainMenu;

    void Awake()
    {
        mainMenu = GetComponentInParent<MainMenu>();
    }

    public void OnSelect(BaseEventData baseEvent)
    {
        mainMenu.UpdateLastSelected(gameObject);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
