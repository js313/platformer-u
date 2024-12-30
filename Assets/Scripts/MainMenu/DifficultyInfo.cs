using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultyInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] TextMeshProUGUI difficultyInfo;
    [TextArea]
    [SerializeField] string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        difficultyInfo.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        difficultyInfo.text = "";
    }

    public void OnSelect(BaseEventData eventData)
    {
        difficultyInfo.text = description;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        difficultyInfo.text = "";
    }
}
