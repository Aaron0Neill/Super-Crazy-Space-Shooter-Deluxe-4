using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOnMenuLoad : MonoBehaviour
{
    AudioManager _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

    void Start()
    {
        _audioManager.Play("Menu_Theme");
    }

    private void OnDestroy()
    {
        if (_audioManager != null)
            _audioManager.Stop("Menu_Theme");
    }
}
