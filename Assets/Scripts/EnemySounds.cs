using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemySounds : MonoBehaviour {
	
	//private AudioSource audioSource;

	//public AudioClip hurt;
	//public AudioClip fire;

	//private void Awake(){
	//	audioSource = GetComponent<AudioSource>();
	//	audioSource.playOnAwake = false;
	//	audioSource.loop = false;
	//}

	////WARNING as of 12-26-2021 this will not work because the Enemy itself is getting set to inactive when defeated.
	//public void PlayHurt(){
	//	audioSource.clip = hurt;

	//	audioSource.Play();
	//}

	//public void PlayFire(){
	//	audioSource.clip = fire;

	//	audioSource.Play();
	//}
}
