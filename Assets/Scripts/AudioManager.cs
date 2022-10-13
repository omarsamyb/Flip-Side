using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	[System.Serializable]
	public class Sound
	{
		public string name;

		[HideInInspector]
		public AudioSource source;
		public AudioClip clip;
		public AudioMixerGroup mixerGroup;
		[Range(0f, 1f)]
		public float volume = .75f;
		[Range(0f, 1f)]
		public float volumeVariance = .1f;
		[Range(.1f, 3f)]
		public float pitch = 1f;
		[Range(0f, 1f)]
		public float pitchVariance = .1f;
		public bool loop = false;
		public bool isPlaying = false;
	}

	public static AudioManager instance;
    public AudioMixerGroup mixerGroup;
    public Sound[] sounds;
	public bool isMuted = false;
    private void Awake()
    {
		if (instance != null)
			Destroy(gameObject);
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;
			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		if (!isMuted)
			s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		else
			s.source.volume = 0;
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
		s.isPlaying = true;
	}

	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Stop();
		s.isPlaying = false;
	}

	public void Pause(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Pause();
	}

	public void UnPause(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.UnPause();
	}

	public bool isPlaying(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return false;
		}
		return s.isPlaying;
	}

	public void ToggleMute()
	{
		if (isMuted)
		{
			foreach (Sound s in sounds)
			{
				s.source.volume = s.volume;
			}
			isMuted = false;
		}
		else
		{
			foreach (Sound s in sounds)
			{
				s.source.volume = 0;
			}
			isMuted = true;
		}
	}
	public void setPitch(string sound, float value)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.pitch = value;
	}
}
