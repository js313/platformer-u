using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultyInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
}
