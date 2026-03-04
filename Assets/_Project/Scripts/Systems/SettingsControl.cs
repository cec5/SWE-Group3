using UnityEngine;
using UnityEngine.UI;

public class SettingsControl : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void OnEnable() 
    {
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        }
    }

    private void OnDisable() // Clean up listeners to avoid duplicates
    {
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        if (sfxSlider != null) sfxSlider.onValueChanged.RemoveListener(OnSFXChanged);
    }

    private void OnMusicChanged(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        if (AudioManager.instance != null)
            AudioManager.instance.SetMusicVolume(volume);
    }

    private void OnSFXChanged(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}