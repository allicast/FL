using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sliderMusic;
    public Slider sliderSFX;

    void Start()
    {
       
        float music = PlayerPrefs.GetFloat("musicVolume", 0.75f);
        float sfx = PlayerPrefs.GetFloat("sfxVolume", 0.75f);

        sliderMusic.value = music;
        sliderSFX.value = sfx;

        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }

    public void SetMusicVolume(float value)
    {
        float dB = Mathf.Log10(value <= 0 ? 0.001f : value) * 20f;
        audioMixer.SetFloat("MusicVolume", dB);
        PlayerPrefs.SetFloat("musicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(value <= 0 ? 0.001f : value) * 20f;
        audioMixer.SetFloat("SFXVolume", dB);
        PlayerPrefs.SetFloat("sfxVolume", value);
    }
}
