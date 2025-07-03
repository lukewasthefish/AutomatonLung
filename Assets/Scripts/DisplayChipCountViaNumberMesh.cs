using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NumberMesh))]
public class DisplayChipCountViaNumberMesh : MonoBehaviour {
	private NumberMesh numberMesh;

	private void Start(){
		numberMesh = GetComponent<NumberMesh>();
	}

	private void Update(){
		numberMesh.numberToDisplay = GameManager.Instance.chipCount;
	}
}
