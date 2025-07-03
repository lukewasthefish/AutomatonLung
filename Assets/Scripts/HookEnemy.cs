using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookEnemy : Enemy {

	private Vector3 destinationScale;
	private Vector3 initialScale;

	protected override void Awake(){
		base.Awake();

		initialScale = this.transform.localScale;
		destinationScale = initialScale;

		InvokeRepeating("Jump", Random.Range(0.1f, 2f), 0.5f);
	}

    public HookEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

	protected override void Update(){
		this.knockback = Vector3.zero;
		this.transform.localScale = Vector3.Lerp(this.transform.localScale, destinationScale, 16f * Time.deltaTime);

		base.Update();
	}

	private void Jump(){
        if (!GameManager.Instance.thirdPersonCharacterMovement)
        {
			return;
        }

		if(destinationScale == initialScale){
			destinationScale = new Vector3(initialScale.x / 1.3f, initialScale.y / 1.3f, initialScale.z * 1.6f);
		} else {
			this.transform.LookAt(GameManager.Instance.thirdPersonCharacterMovement.transform.position);
			this.transform.rotation = new Quaternion(0f, this.transform.rotation.y, 0f, this.transform.rotation.w);
			this.transform.Rotate(-90f, 0f, 0f);

			Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position, 30f);
			destinationScale = initialScale;
		}
	}
}
