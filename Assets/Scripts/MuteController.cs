using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteController : MonoBehaviour
{

    GameObject _audioManager;

    void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager");
    }

    public void ToggleSFX()
    {
        _audioManager.GetComponent<AudioManager>().ToggleSFX();
    }

    public void ToggleMusic()
    {
        _audioManager.GetComponent<AudioManager>().ToggleMusic();
    }
}
