using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Explosion particles, sounds, and mechanics.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour {

	public ParticleSystem explosionParticles;
	public AudioClip explosionSound;

	private AudioSource audioSource;

	public float explosionRadius = 2f;
	public int damageAmmount = 40;

	private void Awake(){
		audioSource = GetComponent<AudioSource>();
		audioSource.playOnAwake = false;
		audioSource.loop = false;
		audioSource.clip = explosionSound;
	}

	public void Explode(){
		audioSource.Play();
		explosionParticles.Play();

		GameManager.Instance.thirdPersonCharacterMovement.GetComponent<ThirdPersonCharacterMovement>().thirdPersonPlayerCamera.cameraShake.Shake(0.5f, 24f);

		Collider[] colliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
		for(int i = 0; i < colliders.Length; i++)
		{
			PlayerCombat pc = colliders[i].GetComponent<PlayerCombat>();

			if(pc!=null){
				this.transform.LookAt(pc.transform.position);

				pc.TakeDamage(damageAmmount, this.transform.forward * 2f);
			}

			Enemy enemy = colliders[i].GetComponent<Enemy>();
			if(enemy){
				enemy.TakeDamage(damageAmmount, this.transform.position - this.transform.forward);
			}

			BreakableBlock bb = colliders[i].GetComponent<BreakableBlock>();
			if(bb){
				bb.Break();
			}
		}
	}
}
