using UnityEngine;

public class SkinSelection : MonoBehaviour
{
    [SerializeField] int skinIndex;
    [SerializeField] int totalSkinCount;
    [SerializeField] Animator skinDisplay;

    public void NextSkin()
    {
        skinIndex = (skinIndex + 1) % totalSkinCount;

        UpdateSkin();
    }

    public void PreviousSkin()
    {
        skinIndex = (skinIndex - 1 + totalSkinCount) % totalSkinCount;

        UpdateSkin();
    }

    public void OnSkinSelect()
    {
        SkinManager.instance.selectedSkinIndex = skinIndex;
    }

    void UpdateSkin()
    {
        for (int i = 0; i < totalSkinCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }
        skinDisplay.SetLayerWeight(skinIndex, 1);
    }
}
