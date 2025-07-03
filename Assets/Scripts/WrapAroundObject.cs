using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAroundObject : MonoBehaviour {

	public float maxDistanceFromStart = 20f;

	public float moveSpeed = 15f;

	public Vector3 moveDirection;
	private Vector3 startPosition;

	// private void Awake(){
	// 	startPosition = this.transform.position;
	// }

	// private void Update(){
	// 	this.transform.position += moveDirection * moveSpeed * Time.deltaTime;

	// 	if(Vector3.Distance(this.transform.position, startPosition) > maxDistanceFromStart){
	// 		this.transform.position = -this.transform.position + (moveDirection*4f);
	// 	}
	// }
}
