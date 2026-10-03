using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [Header("Audio Mixer")] 
    [SerializeField] private AudioMixer audioMixer;

    [Header("UI Sliders")] 
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private const string MUSIC_KEY = "MusicVolumePreference";
    private const string SFX_KEY = "SFXVolumePreference";
    private const string MIXER_MUSIC = "MusicVol";
    private const string MIXER_SFX = "SFXVol";
    void Start()
    {
        float savedMusic = PlayerPrefs.GetFloat(MUSIC_KEY,0.75f);
        float savedSFX = PlayerPrefs.GetFloat(SFX_KEY,0.75f);

        if (musicSlider != null)
        {
            musicSlider.value = savedMusic;
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFX;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
        ApplyVolume(MIXER_MUSIC, savedMusic);
        ApplyVolume(MIXER_SFX, savedSFX);

    }

    public void SetMusicVolume(float value)
    {
        ApplyVolume(MIXER_MUSIC, value);
        PlayerPrefs.SetFloat(MUSIC_KEY,value);
    }

    public void SetSFXVolume(float value)
    {
        ApplyVolume(MIXER_SFX, value);
        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    private void ApplyVolume(string exposedParam, float sliderValue)
    {
        float clampedValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);
        float dB = Mathf.Log10(clampedValue) * 20f;
        audioMixer.SetFloat(exposedParam, dB);
    }

    
}
