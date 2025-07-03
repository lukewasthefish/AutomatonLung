using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownBoss : Boss {

	private Animator anim;

	protected override void Awake(){
		base.Awake();

		anim = GetComponent<Animator>();

		InvokeRepeating("FlightMove", 1f, 4f);
	}

    public ClownBoss(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
        this.isBoss = true;
    }

	protected override void Update(){
		base.Update();
		
		this.transform.LookAt(GameManager.Instance.thirdPersonCharacterMovement.transform.position);
		this.transform.rotation = new Quaternion(0f, this.transform.rotation.y, 0f, this.transform.rotation.w);

		if(Vector3.Distance(this.transform.position, GameManager.Instance.thirdPersonCharacterMovement.transform.position) < 4f){
			// this.transform.position -= this.transform.forward * moveSpeed * Time.deltaTime;
			// this.transform.position -= this.transform.right * (moveSpeed / 3f) * Time.deltaTime;
		} else {
			this.transform.position += this.transform.forward * (moveSpeed / 4f) * Time.deltaTime;
			this.transform.position -= this.transform.right * (moveSpeed / 5f) * Time.deltaTime;
		}
	}

	protected override void BossDeathAction(){
		BulletFirer[] bulletFirers = GetComponentsInChildren<BulletFirer>();

		for(int i = 0; i < bulletFirers.Length; i++){
			Destroy(bulletFirers[i].gameObject);
		}
	}

	private void FlightMove(){
		anim.SetInteger("flightMove", Random.Range(0,5));
	}
}
