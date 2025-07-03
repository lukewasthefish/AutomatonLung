using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobblehead : MonoBehaviour {

	public Transform headBone;

	private Vector3 initialHeadScale;

	private void Awake(){
		initialHeadScale = headBone.transform.localScale;
	}

	private void LateUpdate(){
		if(GameManager.Instance.bobbleHead)
		headBone.transform.localScale = initialHeadScale * 2.5f;
	}
}
