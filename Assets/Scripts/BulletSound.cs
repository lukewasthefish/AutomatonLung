using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class BulletSound : MonoBehaviour {
	public AudioClip[] fireSounds;

	private AudioSource audioSource;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnDisable()
    {
		audioSource.Stop();
    }

	public void PlaySound(int weaponIndex)
	{
		audioSource.clip = fireSounds[weaponIndex];

		audioSource.Play();
	}

	public AudioSource GetAudioSource()
    {
		return audioSource;
    }
}
