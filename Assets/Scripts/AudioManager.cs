using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioManager : MonoBehaviour
{

	private static AudioManager m_Instance;

	public Sound[] sounds;

	void Awake()
	{
		if (m_Instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			m_Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();

			s.source.clip = s.clip;
			s.source.loop = s.loop;

		}
		Play("Menu_Theme");
	}



	/// <summary>
	/// Use to play an audio file
	/// </summary>
	/// <param name="sound">The name of the file you want to play</param>
	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		if (s.state == Sound.State.Enabled)
		{
			s.source.Play();
		}
	}


	/// <summary>
	/// Used to stop an audio manually through code
	/// </summary>
	/// <param name="name">The file name you want to stop</param>
	public void Stop(string name)
	{
		Sound s = Array.Find(m_Instance.sounds, sound => sound.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found! Can't stop it!");
			return;
		}
		s.source.Stop();
	}

	/// <summary>
	/// Used to change the volume of Music files through options
	/// </summary>
	public void SetMusicVolume(float t_volume)
	{
		foreach (Sound s in sounds)
		{
			if (s.typeOfAudio == Sound.AudioType.Music)
			{
				s.source.volume = t_volume;
			}
		}
	}

	/// <summary>
	/// Used to change the volume of sfx files through options
	/// </summary>
	public void SetSFXVolume(float t_volume)
	{
		foreach (Sound s in sounds)
		{
			if (s.typeOfAudio == Sound.AudioType.SFX)
			{
				s.source.volume = t_volume;
			}
		}
	}


	/// <summary>
	/// Used to stop an audio file through the buttons in option menu
	/// </summary>
	public void ToggleMusic()
	{
		foreach (Sound s in sounds)
		{
			if (s.typeOfAudio == Sound.AudioType.Music && s.state == Sound.State.Enabled)
			{
				s.source.mute = true;
				s.state = Sound.State.Disabled;
			}
			else if (s.typeOfAudio == Sound.AudioType.Music && s.state == Sound.State.Disabled)
			{
				s.source.mute = false;
				s.state = Sound.State.Enabled;
			}
		}
	}

	public void ToggleSFX()
	{
		foreach (Sound s in sounds)
		{
			if (s.typeOfAudio == Sound.AudioType.SFX && s.state == Sound.State.Enabled)
			{
				s.source.mute = true;
				s.state = Sound.State.Disabled;
			}
			else if (s.typeOfAudio == Sound.AudioType.SFX && s.state == Sound.State.Disabled)
			{
				s.source.mute = false;
				s.state = Sound.State.Enabled;
			}
		}
	}

	public float GetCurrentMusicVolume()
	{
		foreach (Sound s in sounds)
		{
			if (s.typeOfAudio == Sound.AudioType.Music)
			{
				return s.source.volume;
			}
		}
		return 0;
	}

	public float GetCurrentSFXVolume()
	{
		foreach (Sound s in sounds)
		{
			if (s.typeOfAudio == Sound.AudioType.SFX)
			{
				return s.source.volume;
			}
		}
		return 0;
	}

	//----------------------------------------- Functions added since first iteration below ---------------------------------------------------------
	// A function to allow us to play click SFX during menu naviagtion.
	public void playMenuSFX()
    {
		Play("Menu_Click");
    }

	public void MenuTransition()
    {
		Sound s = Array.Find(m_Instance.sounds, AudioFile => AudioFile.name == "Menu_Theme");
		Stop("Menu_Theme");
	}
}