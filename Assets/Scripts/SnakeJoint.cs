using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeJoint : MonoBehaviour {
	public Transform nextJoint;
	public float distanceBehindNextJoint = 2f;
	public float rotationSlerpSpeed = 2f;

	private void Update(){
		if(!nextJoint){
			return;
		}

		this.transform.position = nextJoint.transform.position + (nextJoint.transform.up * distanceBehindNextJoint);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, nextJoint.transform.rotation, rotationSlerpSpeed * Time.deltaTime);
	}
}
