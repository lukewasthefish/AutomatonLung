using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckAngelEnemy : Enemy {

    public DuckAngelEnemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }
	
    protected override void Awake()
    {
		base.Awake();
		CancelInvoke ("BoostInNewDirection");
		InvokeRepeating("BoostInNewDirection", 1f, Random.Range(0.4f, 0.7f));
    }

    private void OnEnable()
    {
        this.knockback = Vector3.zero;
    }

    protected override void Update()
    {
		base.Update();

		if(GameManager.Instance && GameManager.Instance.thirdPersonCharacterMovement)
			this.transform.LookAt(GameManager.Instance.thirdPersonCharacterMovement.transform.position);

		this.transform.position += this.transform.forward * moveSpeed * Time.deltaTime;
    }

	private void BoostInNewDirection(){
		int roll = Random.Range(0,5);
		float knockBackMultiplier = 1f;

		switch(roll){
			case 0:
				knockback = this.transform.right * knockBackMultiplier;
				return;
			case 1:
				knockback = this.transform.forward * knockBackMultiplier;
				return;
			case 2:
				knockback = -this.transform.right * knockBackMultiplier;
				return;
			case 3:
				if(GameManager.Instance.thirdPersonCharacterMovement)
					Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position, 20f);
				knockback = -this.transform.forward * knockBackMultiplier;
				return;
			case 4:
				return;
		}
	}
}
