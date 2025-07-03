using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour {

	public ParticleSystem breakParticles;

	private Collider thisCollider;
	private Renderer thisRenderer;

	private AudioSource audioSource;

	private void Awake(){
		audioSource = GetComponent<AudioSource>();
		thisCollider = GetComponent<Collider>();
		thisRenderer = GetComponent<Renderer>();
	}

	public void Break(){
		breakParticles.Play();
		audioSource.Play();

		thisRenderer.enabled = false;
		thisCollider.enabled = false;

		if(this.GetComponent<CullThisObject>()){
			this.gameObject.GetComponent<CullThisObject>().cullingEnabled = false;
		}
	}
}
