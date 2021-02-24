using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound {

	public enum AudioType
	{
		Music = 0,
		SFX = 1
	};

    public enum State
    {
		Enabled = 2,
		Disabled = 3
	}

	public string name;

	public AudioType typeOfAudio;

	public State state;

	public AudioClip clip;

	public bool loop = true;

	[HideInInspector]
	public AudioSource source;

}
