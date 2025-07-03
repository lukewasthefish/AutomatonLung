using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionToZero : MonoBehaviour {
	private void Awake(){
		this.transform.position = Vector3.zero;
	}
}
