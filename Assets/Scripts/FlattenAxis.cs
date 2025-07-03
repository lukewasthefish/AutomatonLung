using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlattenAxis : MonoBehaviour {

	public bool flattenX = false;
	public bool flattenY = false;
	public bool flattenZ = false;

	private void LateUpdate(){
		if(flattenX){
			this.transform.eulerAngles = new Vector3(-90f, this.transform.eulerAngles.y, this.transform.eulerAngles.z);
		}

		if(flattenY){
			this.transform.eulerAngles = new Vector3(this.transform.rotation.eulerAngles.x, 0f, this.transform.eulerAngles.z);
		}

		if(flattenZ){
			this.transform.eulerAngles = new Vector3(this.transform.rotation.eulerAngles.x, this.transform.eulerAngles.y, 0f);
		}
	}
}
