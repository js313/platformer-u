using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[Serializable]
struct Skin
{
    public int skinPrice;
    public string skinName;
    public bool unlocked;
}

public class SkinSelection : MonoBehaviour
{
    [SerializeField] GameObject firstSelected;
    DefaultInputActions inputActions;

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
        inputActions = new DefaultInputActions();
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
        menu.UpdateLastSelected(firstSelected);
        EventSystem.current.SetSelectedGameObject(firstSelected);
        inputActions.Enable();
        inputActions.UI.Navigate.performed += ctx =>
        {
            if (ctx.ReadValue<Vector2>().x <= -1) PreviousSkin();
            else if (ctx.ReadValue<Vector2>().x >= 1) NextSkin();
        };
    }

    private void OnDisable()
    {
        inputActions.Disable();
        inputActions.UI.Navigate.performed -= ctx =>
        {
            if (ctx.ReadValue<Vector2>().x <= -1) PreviousSkin();
            else if (ctx.ReadValue<Vector2>().x >= 1) NextSkin();
        };
    }

    public void NextSkin()
    {
        AudioManager.instance.PlaySfx(4);

        skinIndex = (skinIndex + 1) % totalSkinCount;
        UpdateSkin();
    }

    public void PreviousSkin()
    {
        AudioManager.instance.PlaySfx(4);

        skinIndex = (skinIndex - 1 + totalSkinCount) % totalSkinCount;
        UpdateSkin();
    }

    public void OnSkinSelect(GameObject uiElementToEnable)
    {
        if (skins[skinIndex].unlocked)
        {
            AudioManager.instance.PlaySfx(4);

            SkinManager.instance.selectedSkinIndex = skinIndex;
            PlayerPrefs.SetInt("LastSkinPlayed", skinIndex);
            menu.SwitchUI(uiElementToEnable);
            menu.MoveCameraToMainMenu();
        }
        if (!skins[skinIndex].unlocked)
        {
            int remainingBankBalance = PlayerPrefs.GetInt("FruitsInBank") - skins[skinIndex].skinPrice;
            if (remainingBankBalance < 0)
            {
                AudioManager.instance.PlaySfx(6);

                return;
            }

            AudioManager.instance.PlaySfx(10);

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
