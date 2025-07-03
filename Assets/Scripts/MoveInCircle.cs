using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInCircle : MonoBehaviour {

	public float forwardSpeed = 100f;
	public float rotateSpeed = 50f;

	private void Update () {
		this.transform.position += this.transform.forward * forwardSpeed * Time.deltaTime;
		this.transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
	}
}
