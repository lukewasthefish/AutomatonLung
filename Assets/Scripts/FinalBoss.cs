using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : Boss {

	private GameObject lookLocation;
	private Vector3 actualLookLocation;

	private bool secondPhaseStarted = false;

    public FinalBoss(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

    protected override void Start()
    {
		base.Start();

		lookLocation = Instantiate(new GameObject("finalBossLookLocation"));
		lookLocation.transform.position = GameManager.Instance.thirdPersonCharacterMovement.transform.position;
		actualLookLocation = lookLocation.transform.position;

		InvokeRepeating("SuddenMovement", 1f, 0.2f);
    }

    protected override void Update()
    {
        base.Update();
		this.transform.LookAt(actualLookLocation);

		ConstantMovement();

		if(!secondPhaseStarted && currentHealth < maxHealth/2f){
			secondPhaseStarted = true;
		}
    }

    protected override void BossDeathAction()
    {     
        LoadSceneAfterDefeat();
    }

	private void SuddenMovement(){
		lookLocation.transform.position = GameManager.Instance.thirdPersonCharacterMovement.transform.position + (Random.insideUnitSphere*24f);

		knockback += Random.insideUnitSphere;

		if(secondPhaseStarted){
			knockback += Random.insideUnitSphere * 5f;
			Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position, 40f);
		}
	}

	private void ConstantMovement(){
		bool nearPlayer = Vector3.Distance(this.transform.position, GameManager.Instance.thirdPersonCharacterMovement.transform.position) < 38f;

		if(nearPlayer){
			knockback -= this.transform.forward * 4f * Time.deltaTime;
		} else {
			knockback += this.transform.forward * 12f * Time.deltaTime;
		}
		actualLookLocation = Vector3.Lerp(actualLookLocation, lookLocation.transform.position, 12f * Time.deltaTime);

		this.transform.position = new Vector3(this.transform.position.x, GameManager.Instance.thirdPersonCharacterMovement.transform.position.y, this.transform.position.z);
		this.transform.rotation = new Quaternion(0f, this.transform.rotation.y, 0f ,this.transform.rotation.w);

		if(secondPhaseStarted){
			knockback += Random.insideUnitSphere * 2f * Time.deltaTime;
		}
	}

	public override void TakeDamage(int damageAmmount, Vector3 attackDirection){
		base.TakeDamage(damageAmmount, attackDirection);

		Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position, 60f);
		Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position + GameManager.Instance.thirdPersonCharacterMovement.transform.position * 1.5f, 45f);
		Fire(GameManager.Instance.thirdPersonCharacterMovement.transform.position - GameManager.Instance.thirdPersonCharacterMovement.transform.position * 1.5f, 35f);

		knockback -= new Vector3(this.transform.position.x - attackDirection.x, 0f, this.transform.position.z - attackDirection.z);
	}
}
