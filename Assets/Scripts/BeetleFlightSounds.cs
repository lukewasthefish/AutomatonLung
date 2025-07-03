using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BeetleFlightSounds : MonoBehaviour {
	private AudioSource audioSource;

	public AudioClip flightEngine;

	private float engineVolume = 0f;

	private void Awake(){
		audioSource = GetComponent<AudioSource>();
	}

	public void FlightEngine(float flightSpeed){
		float destinationVolume = (flightSpeed > 0.05f) ? 1f : 0f;

		engineVolume = Mathf.Lerp(engineVolume, destinationVolume, 4f * Time.deltaTime);

		if(!audioSource.isPlaying){
		audioSource.clip = flightEngine;
		UpdateAudioSourceValues(3f + (flightSpeed) + Random.Range(0f,0.4f), engineVolume);
		audioSource.Play();
		}
	}

	private void UpdateAudioSourceValues(float pitch, float volume){
		audioSource.pitch = pitch;
		audioSource.volume = volume;
	}
}
