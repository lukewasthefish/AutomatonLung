using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Horseshoe crabs are not our enemies
/// </summary>
public class HorseshoeCrabEenemy : Enemy {
	private bool flipped = false;

	private Transform destinationTransform;

	private float turnBias = 0f;

	private Quaternion initialRotation;

	protected override void Awake(){
		base.Awake();

		initialRotation = this.transform.rotation;

		destinationTransform = new GameObject("HorseshoeCrab transform destination").transform;

		Initialize();
	}

	private void OnEnable(){
		Initialize();
	}

	private void Initialize(){
		this.transform.rotation = initialRotation;

		destinationTransform.transform.rotation = this.transform.rotation;

		flipped = Mathf.RoundToInt(Random.Range(0f, 1f)) == 1;

		turnBias = Random.Range(0f, 60f);
		if(flipped){
			destinationTransform.Rotate(180f, 0f, 0f);
		}
	}

    public HorseshoeCrabEenemy(CharacterController cc, MeshFlash mf) : base(cc, mf)
    {
    }

	protected override void Update(){
		base.Update();

		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, destinationTransform.transform.rotation, 10f * Time.deltaTime);

		if(!flipped){
			HorseshoeCrabMovement();
		}
	}

	public override void TakeDamage(int damageAmmount, Vector3 attackDirection){
		if(!this.flipped){
			knockback += new Vector3(this.transform.position.x - attackDirection.x, 0f, this.transform.position.z - attackDirection.z);
			return;
		}

		base.TakeDamage(damageAmmount, attackDirection);

		this.currentFallSpeed = -10f;

		this.flipped = false;
		destinationTransform.Rotate(180f, 0f, 0f);
	}

	private void HorseshoeCrabMovement(){
		this.transform.Rotate(0f, 0f, Random.Range(-30f + turnBias,30f + turnBias) * 4f * Time.deltaTime);
		this.knockback = -this.transform.right / 8f;
	}
}
