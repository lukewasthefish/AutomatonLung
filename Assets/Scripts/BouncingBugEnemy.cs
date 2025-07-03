using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBugEnemy : Enemy {

	private bool waitingForJump = false;

    public BouncingBugEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

    private void OnEnable()
    {
        this.knockback = Vector3.zero;
    }

    protected override void Update()
    {
		base.Update();

		if(!waitingForJump && currentFallSpeed >= maxFallSpeed){
			Jump();
			Invoke("ResetJumpWait", 0.4f);
			waitingForJump = true;
		}
    }

	private void ResetJumpWait(){
		waitingForJump = false;
	}

	private void Jump(){
		currentFallSpeed = -currentFallSpeed;
		knockback += this.transform.forward;

		this.transform.LookAt(GameManager.Instance.thirdPersonCharacterMovement.transform.position);

		//this.transform.Rotate(0f, Random.Range(-45f,45f), 0f);
	}
}
