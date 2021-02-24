using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOnLevelLoad : MonoBehaviour
{
    GameObject _audioManager;

    private void Awake()
    {
        _audioManager = GameObject.FindGameObjectWithTag("AudioManager");
    }

    void Start()
    {
        _audioManager.GetComponent<AudioManager>().Play("Game_Theme");
    }

    private void OnDestroy()
    {
        if(_audioManager != null)
        _audioManager.GetComponent<AudioManager>().Stop("Game_Theme");
    }


}
