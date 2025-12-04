using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Cargar valores guardados
        musicSlider.value = PlayerPrefs.GetFloat("musicVol", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVol", 1f);

        // Aplicarlos
        musicSource.volume = musicSlider.value;
        sfxSource.volume = sfxSlider.value;

        // Listener
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float v)
    {
        musicSource.volume = v;
        PlayerPrefs.SetFloat("musicVol", v);
    }

    public void SetSFXVolume(float v)
    {
        sfxSource.volume = v;
        PlayerPrefs.SetFloat("sfxVol", v);
    }
}
