using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float mixerMultiplier = 20;

    [Header("SFX Settings")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] TextMeshProUGUI sfxPercent;
    [SerializeField] string sfxParameter;
    float sfxValue;

    [Header("Music Settings")]
    [SerializeField] Slider musicSlider;
    [SerializeField] TextMeshProUGUI musicPercent;
    [SerializeField] string musicParameter;
    float musicValue;

    public void SfxSliderValue(float value)
    {
        sfxPercent.text = (int)(value * 100) + "%";
        sfxValue = value;
        audioMixer.SetFloat(sfxParameter, Mathf.Log10(value) * mixerMultiplier);
    }

    public void MusicSliderValue(float value)
    {
        musicPercent.text = (int)(value * 100) + "%";
        musicValue = value;
        audioMixer.SetFloat(musicParameter, Mathf.Log10(value) * mixerMultiplier);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxValue);
        PlayerPrefs.SetFloat("musicVolume", musicValue);
    }

    private void OnEnable()
    {
        sfxValue = PlayerPrefs.GetFloat("sfxVolume", 1);
        musicValue = PlayerPrefs.GetFloat("musicVolume", 1);
        SfxSliderValue(sfxValue);
        MusicSliderValue(musicValue);
        sfxSlider.value = sfxValue;
        musicSlider.value = musicValue;
    }
}
