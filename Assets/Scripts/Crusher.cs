using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crusher : MonoBehaviour {

	public bool isActive = false;

	public float movementSpeed = 10f;

	private void Update(){
		if(!isActive){
			return;
		}

		this.transform.position += Vector3.down * movementSpeed * Time.deltaTime;
		
		if(Physics.Raycast(this.transform.position, -this.transform.up, this.transform.localScale.y / 1.5f)){
			isActive = false;
		}
	}

	public void Activate(){
		isActive = true;
	}

	public void DeActivate(){
		isActive = false;
	}
}
