using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnEnable : MonoBehaviour {

	private AudioSource audioSource;

	private void Awake(){
		audioSource = GetComponent<AudioSource>();
	}

	private void OnEnable(){
		audioSource = GetComponent<AudioSource>();
		audioSource.Stop();
		audioSource.Play();
	}
}
