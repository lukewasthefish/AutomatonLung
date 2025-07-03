using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormEnemy : Enemy {

	Vector3 lookPosition;

	bool hasNoticedPlayer = false;

	protected override void Awake(){
		CancelInvoke("Jump");
		InvokeRepeating("Jump", Random.Range(1f, 3f), 0.4f);
	}

    public WormEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

	protected override void Update(){
		base.Update();

		if(!hasNoticedPlayer){
			Invoke("NoticePlayer", Random.Range(0.2f, 12f));

			if(currentHealth < maxHealth){
				NoticePlayer();
			}
		} else {
			lookPosition = GameManager.Instance.thirdPersonCharacterMovement.transform.position;

			this.transform.LookAt(lookPosition);
		}

		baseMoveDirection = this.transform.forward * moveSpeed;
	}

	private void NoticePlayer(){
		hasNoticedPlayer = true;
	}

	private void Jump(){
		this.currentFallSpeed = -15f;

		this.knockback += this.transform.forward * 0.3f;
	}
}
