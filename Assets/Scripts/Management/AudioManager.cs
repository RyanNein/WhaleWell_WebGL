using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : NeinUtility.PersistentSingleton<AudioManager>
{
	[SerializeField] AudioMixer myMixer;
	[SerializeField] AudioSource musicSource;

	List<AudioSource> sources = new List<AudioSource>();
	
	const int MAX_SOURCES = 10;
	const float DEFAULT_VOLUME = .9f;

	float volumeOnPause;
	float volumeDuringPause = 0.25f;


	public enum AudioGroups { 
		MasterGroup, 
		MusicGroup, 
		SfxGroup 
	}

	#region MIXER AND GROUPS

	[SerializeField] private AudioMixerGroup _masterGroup;
	public AudioMixerGroup MasterGroup => _masterGroup;

	[SerializeField] private AudioMixerGroup _musicGroup;
	public AudioMixerGroup MusicGroup => _musicGroup;

	[SerializeField] private AudioMixerGroup _sfxGroup;
	public AudioMixerGroup SfxGroup => _sfxGroup;

	public float CurrentMasterVolume 
	{
		get
		{
			float volume;
			myMixer.GetFloat("MasterVolume", out volume);
			return volume;
		}
	}

	public float CurrentMusicVolume
	{
		get
		{
			float volume;
			myMixer.GetFloat("MusicVolume", out volume);
			return volume;
		}
	}

	public float CurrentSfxVolume
	{
		get
		{
			float volume;
			myMixer.GetFloat("SfxVolume", out volume);
			return volume;
		}
	}

	#endregion

	public void PlayMusic(AudioClip _clip, float _volume = 0.95f)
	{
		if (_clip == musicSource.clip)
			return;

		musicSource.clip = _clip;
		musicSource.outputAudioMixerGroup = MusicGroup;
		musicSource.volume = _volume;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void PlaySFXOneShot(AudioClip _clip, float _volume = 1f)
	{
		bool played = false;

		// Find unused source:
		for (int i = 0; i < sources.Count; i++)
		{
			var source = sources[i];
			if (!source.isPlaying)
			{
				source.outputAudioMixerGroup = SfxGroup;
				source.PlayOneShot(_clip, _volume);
				played = true;
				break;
			}
		}

		// Make new source:
		if (!played && sources.Count < MAX_SOURCES)
		{
			var newSource = gameObject.AddComponent<AudioSource>();
			newSource.outputAudioMixerGroup = SfxGroup;
			newSource.PlayOneShot(_clip, _volume);
			sources.Add(newSource);
		}
	}


	public void SetGroupVolume(AudioGroups _group, float _volume)
	{
		string parameterName = "MasterVolume";

		switch (_group)
		{
			case AudioGroups.MasterGroup:
				parameterName = "MasterVolume";
				break;
			case AudioGroups.MusicGroup:
				parameterName = "MusicVolume";
				break;
			case AudioGroups.SfxGroup:
				parameterName = "SfxVolume";
				break;
		}

		myMixer.SetFloat(parameterName, _volume);
	}

	public float GetGroupVolume(AudioGroups _group)
	{
		string parameterName = "MasterVolume";
		float currentVolume;

		switch (_group)
		{
			case AudioGroups.MasterGroup:
				parameterName = "MasterVolume";
				break;
			case AudioGroups.MusicGroup:
				parameterName = "MusicVolume";
				break;
			case AudioGroups.SfxGroup:
				parameterName = "SfxVolume";
				break;
		}

		myMixer.GetFloat(parameterName, out currentVolume);
		return currentVolume;
	}

	public void StopMusic()
	{
		musicSource.Pause();
	}

	public void RestartMusic()
	{
		if (musicSource.clip != null)
			musicSource.Play();
	}

	// for pause
	private void LowerVolume()
	{
		volumeOnPause = CurrentMasterVolume;
		SetGroupVolume(AudioGroups.MasterGroup, volumeDuringPause);
	}

	// for pause
	private void ResetVolume()
	{
		SetGroupVolume(AudioGroups.MasterGroup, volumeOnPause);
	}
}
