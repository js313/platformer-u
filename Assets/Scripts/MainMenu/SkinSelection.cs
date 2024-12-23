using System;
using TMPro;
using UnityEngine;

[Serializable]
struct Skin
{
    public int skinPrice;
    public string skinName;
    public bool unlocked;
}

public class SkinSelection : MonoBehaviour
{
    MainMenu menu;

    [SerializeField] int skinIndex;
    [SerializeField] int totalSkinCount;
    [SerializeField] Animator skinDisplay;

    [SerializeField] Skin[] skins;
    [SerializeField] GameObject skinPriceDisplayParent;
    [SerializeField] TextMeshProUGUI skinPriceDisplay;
    [SerializeField] TextMeshProUGUI bankBalanceDisplay;
    [SerializeField] TextMeshProUGUI skinSelectButtonText;

    private void Awake()
    {
        menu = transform.parent.GetComponentInChildren<MainMenu>();
    }

    private void Start()
    {
        for (int i = 0; i < skins.Length; i++)
        {
            skins[i].unlocked = (PlayerPrefs.GetInt("Skin" + skins[i].skinName + "Unlocked") == 1);
        }
        UpdateSkinDisplay();
        UpdateBankBalanceDisplay();
    }

    private void OnEnable()
    {
        UpdateSkin();
    }

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

    public void OnSkinSelect(GameObject uiElementToEnable)
    {
        if (skins[skinIndex].unlocked)
        {
            SkinManager.instance.selectedSkinIndex = skinIndex;
            PlayerPrefs.SetInt("LastSkinPlayed", skinIndex);
            menu.SwitchUI(uiElementToEnable);
            menu.MoveCameraToMainMenu();
        }
        if (!skins[skinIndex].unlocked)
        {
            int remainingBankBalance = PlayerPrefs.GetInt("FruitsInBank") - skins[skinIndex].skinPrice;
            if (remainingBankBalance < 0) return;

            PlayerPrefs.SetInt("FruitsInBank", remainingBankBalance);
            PlayerPrefs.SetInt("Skin" + skins[skinIndex].skinName + "Unlocked", 1);
            skins[skinIndex].unlocked = true;
            UpdateBankBalanceDisplay();
            UpdateSkinDisplay();
        }
    }

    void UpdateSkin()
    {
        for (int i = 0; i < totalSkinCount; i++)
        {
            skinDisplay.SetLayerWeight(i, 0);
        }
        skinDisplay.SetLayerWeight(skinIndex, 1);
        UpdateSkinDisplay();
    }

    void UpdateSkinDisplay()
    {
        bool isSkinUnlocked = (PlayerPrefs.GetInt("Skin" + skins[skinIndex].skinName + "Unlocked") == 1);
        skinPriceDisplayParent.SetActive(!isSkinUnlocked);
        skinPriceDisplay.text = "Cost: " + skins[skinIndex].skinPrice;
        if (isSkinUnlocked)
            skinSelectButtonText.text = "Select";
        else
            skinSelectButtonText.text = "Unlock";
    }

    void UpdateBankBalanceDisplay()
    {
        bankBalanceDisplay.text = "Bank: " + PlayerPrefs.GetInt("FruitsInBank");
    }
}
