using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FakeFog))]
public class AssignAsFakeFog : MonoBehaviour {

	private void Start(){
		Invoke("Assign", 0.4f);
	}

	private void Assign()
    {
		GameManager.Instance.fakeFog = this.gameObject;
	}
}
