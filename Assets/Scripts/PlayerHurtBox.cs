using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtBox : MonoBehaviour {

	public int damageAmmount = 2;
	public float launchMultiplier = 1f;

	private void OnTriggerEnter(Collider other){
		if(other.GetComponent<PlayerCombat>() != null){
			PlayerCombat pc = other.GetComponent<PlayerCombat>();

			pc.TakeDamage(damageAmmount, (this.transform.forward + (Vector3.up/2f)) * launchMultiplier);

			if(this.GetComponent<Enemy>()){
				LaunchSelfBack(this.GetComponent<Enemy>());
			} else if (this.GetComponentInParent<Enemy>()){
				LaunchSelfBack(this.GetComponentInParent<Enemy>());
			}
		}
	}

	private void LaunchSelfBack(Enemy self){
		self.knockback -= self.transform.forward * 3f;
	}
}
