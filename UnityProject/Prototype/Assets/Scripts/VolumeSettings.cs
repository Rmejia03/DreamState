using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider audioSlider;
    [SerializeField] private Slider SFX;
    private void Start()
    {
        if (PlayerPrefs.HasKey("auioVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }
    }
    public void SetMusicVolume()
    {
        float volume = audioSlider.value;
        audioMixer.SetFloat("menuSound", volume);
        PlayerPrefs.SetFloat("audioVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = SFX.value;
        audioMixer.SetFloat("soundFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXvolume", volume);
    }

    private void LoadVolume()
    {
        audioSlider.value = PlayerPrefs.GetFloat("audioVolume");
        SFX.value = PlayerPrefs.GetFloat("SFXvolume");
        SetMusicVolume();
        SetSFXVolume();
    }
}
