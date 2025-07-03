using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour {
	public RotateObject rotateObject;

	private Collider thisCollider;

	public bool startActive = true;

	private bool isTargetInBox = false;

	private ThirdPersonCharacterMovement thirdPersonCharacterMovement;

	private void Start(){
		thisCollider = GetComponent<Collider>();

		rotateObject.enabled = startActive;
		thisCollider.enabled = startActive;
	}

	private void OnTriggerStay(Collider other){
		thirdPersonCharacterMovement = other.GetComponent<ThirdPersonCharacterMovement>();
		isTargetInBox = true;
	}

	private void OnTriggerExit(){
		isTargetInBox = false;
	}

	private void Update(){
		if(isTargetInBox && thirdPersonCharacterMovement){
			thirdPersonCharacterMovement.dashVector += Vector3.up * 100f * Time.deltaTime;
		}
	}

	public void TurnOn(){
		rotateObject.enabled = true;
		thisCollider.enabled = true;
	}

	public void TurnOff(){
		rotateObject.enabled = false;
		thisCollider.enabled = false;
	}

	public void ToggleOn(){
		rotateObject.enabled = !rotateObject.enabled;
		thisCollider.enabled = !thisCollider.enabled;
	}
}
