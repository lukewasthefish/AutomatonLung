using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSounds : MonoBehaviour {

	private AudioSource audioSource;

	public AudioClip[] hurtSounds;
	public AudioClip hoverBoardSound;
	public AudioClip footStep;
	public AudioClip boost;
	public AudioClip teleport;

	private float hoverboardVolume = 0f;

	private void Awake(){
		audioSource = GetComponent<AudioSource>();
		audioSource.loop = false;
		audioSource.playOnAwake = false;
	}

	public void PlayHurt(){
		audioSource.clip = hurtSounds[Random.Range(0, hurtSounds.Length)];

		UpdateAudioSourceValues(Random.Range(0.7f, 1.2f), 1f);

		audioSource.Play();
	}

	public void PlayHoverBoard(float hoverboardSpeed){
		float hoverboardDestinationVolume = (hoverboardSpeed > 0.1f) ? 1f : 0f;

		hoverboardVolume = Mathf.Lerp(hoverboardVolume, hoverboardDestinationVolume, 4f * Time.deltaTime);

		if(!audioSource.isPlaying){
		audioSource.clip = hoverBoardSound;
		UpdateAudioSourceValues(3f + (hoverboardSpeed) + Random.Range(0f,0.4f), hoverboardVolume);
		audioSource.Play();
		}
	}

	public void PlayFootStep(){
		audioSource.clip = footStep;

		UpdateAudioSourceValues(Random.Range(0.7f, 1.2f), 0.6f);

		audioSource.Play();
	}

	public void PlayBoost(){
		audioSource.clip = boost;

		UpdateAudioSourceValues(Random.Range(1f,1.2f), 1f);

		audioSource.Play();
	}

	public void PlayTeleport()
    {
		if (!audioSource.isPlaying)
		{
			audioSource.clip = teleport;
			UpdateAudioSourceValues(Random.Range(0.9f, 1.1f), 0.3f);
			audioSource.Play();
		}
	}

	private void UpdateAudioSourceValues(float pitch, float volume){
		audioSource.pitch = pitch;
		audioSource.volume = volume;
	}
}
