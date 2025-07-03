using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour {

	public AudioClip musicToPlay;

    private AudioSource audioSource;

	public bool isLooping = true;
    [HideInInspector]public float volumeDestination;

	private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        audioSource.priority = 0;

        if(!audioSource.clip){
            audioSource.clip = musicToPlay;
        }
        
        audioSource.loop = isLooping;
        audioSource.volume = 0f;

        GameManager.Instance.currentMusicPlayer = this;

        audioSource.Play();
    }

    private const float musicVolume = 0.85f;
    private void Update()
    {
        audioSource.volume = Mathf.Lerp(audioSource.volume, volumeDestination, 3f * Time.deltaTime);

        if (GameManager.Instance.musicEnabled)
        {
            volumeDestination = musicVolume;
        } else
        {
            volumeDestination = 0f;
        }
    }
}
