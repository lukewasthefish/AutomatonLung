using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerCharacterAudio : MonoBehaviour {

	public AudioClip[] grunts;

	public AudioClip[] chomps;

	public AudioClip crownGet;

	public AudioClip death;

	private AudioSource audioSource;

	//Used for allowing a player sound to play to the end
	private bool pauseOtherAudio = false;

	private bool deathBool = false;

	private void Awake()
	{
		if(audioSource == null)
		{
			audioSource = this.gameObject.AddComponent<AudioSource>();
		}
	}

	public void Grunt()
	{
		if (pauseOtherAudio == false)
		{
			audioSource.clip = grunts[Mathf.RoundToInt(Random.Range(0, grunts.Length))];
			audioSource.pitch = Random.Range(0.8f, 1.1f);
			audioSource.Play();
		}
	}

	public void Chomp()
	{
		if (pauseOtherAudio == false)
		{
			audioSource.clip = chomps[Mathf.RoundToInt(Random.Range(0, chomps.Length))];
			audioSource.pitch = Random.Range(0.8f, 1.1f);
			audioSource.Play();
		}
	}

	public void CrownGet()
	{
		pauseOtherAudio = true;
		audioSource.clip = crownGet;
		audioSource.pitch = 1;
		audioSource.Play();
	}

	public void Death()
	{
		if (!audioSource.isPlaying && !deathBool)
		{
			pauseOtherAudio = true;
			audioSource.clip = death;
			audioSource.pitch = 1;
			audioSource.Play();
			deathBool = true;
		}
	}

	private void Update()
	{
		if (!audioSource.isPlaying)
		{
			pauseOtherAudio = false;
		}
	}
}
