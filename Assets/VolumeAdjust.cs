using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class VolumeAdjust : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private TextMeshProUGUI musicValue;
    [SerializeField] private TextMeshProUGUI soundValue;
    [SerializeField] AudioMixer audioMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MusicVolume") || PlayerPrefs.HasKey("SoundVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume(musicSlider.value);
            SetSoundVolume(soundSlider.value);
        }
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        SetMusicVolume(musicSlider.value);
        SetSoundVolume(soundSlider.value);
    }

    public void SetMusicVolume(float value)
    {
        musicValue.text = Mathf.RoundToInt(value * 100).ToString();
        audioMixer.SetFloat("Music", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSoundVolume(float value)
    {
        soundValue.text = Mathf.RoundToInt(value * 100).ToString();
        audioMixer.SetFloat("Sound", Mathf.Log10(value) * 20);
        audioMixer.SetFloat("Ambience", Mathf.Log10(value) * 20);
        audioMixer.SetFloat("UI", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SoundVolume", value);
    }
}
