using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFirer : MonoBehaviour {

	public float startupTime = 1f;
	public float repeateRate = 1f;
	public float bulletSpeed = 7f;
	public float bulletScale = 1f;
	public int damageAmmount = 4;

	public bool targetPlayer = false;
	public bool moveFromForwardPosition = false;

	private void Awake(){
		InvokeRepeating("Fire", startupTime, repeateRate);
	}

	private void Fire(){
        Bullet currentBullet = GameManager.Instance.bulletPool.GetObject().GetComponent<Bullet>();
        currentBullet.isEnemyBullet = true;

		currentBullet.damageOutput = damageAmmount;

        currentBullet.transform.position = this.transform.position;

		if(targetPlayer){
        	currentBullet.transform.LookAt(GameManager.Instance.thirdPersonCharacterMovement.transform.position);
		}

		if(moveFromForwardPosition){
			currentBullet.transform.forward = this.transform.forward;
		}

        currentBullet.destructionParticles.Stop();

        currentBullet.speed = bulletSpeed;
		currentBullet.scale = bulletScale;
	}
}
