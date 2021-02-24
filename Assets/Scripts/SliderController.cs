using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    AudioManager _audioManager;
    float _musicLevel;
    float _sfxLevel;

    void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        _musicLevel = _audioManager.GetCurrentMusicVolume();
        _sfxLevel = _audioManager.GetCurrentSFXVolume();

        GetComponent<Slider>().value = (this.gameObject.name == "MusicSlider") ? _musicLevel : _sfxLevel;
    }

    public void setMusicVolume(float t_volume)
    {
        _audioManager.SetMusicVolume(t_volume);
    }

    public void setSFXVolume(float t_volume)
    {
        _audioManager.SetSFXVolume(t_volume);
    }
}
