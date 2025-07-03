using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOnStart : MonoBehaviour {
	public Vector3 rotateAmmount;

	public bool continueThroughUpdate = false;

	private void Start(){
		ApplyRotation();
	}

	private void LateUpdate(){
		if(!continueThroughUpdate){
			return;
		}

		ApplyRotation();
	}

	private void ApplyRotation(){
		this.transform.Rotate(rotateAmmount);
	}
}
