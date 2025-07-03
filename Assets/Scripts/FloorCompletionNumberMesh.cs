using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NumberMesh))]
public class FloorCompletionNumberMesh : MonoBehaviour {
	private NumberMesh numberMesh;

	public MouseUIInteractable mouseUIInteractable;

	private void Awake(){
		numberMesh = GetComponent<NumberMesh>();
	}

	private void InitializingFinished(){
		numberMesh.numberToDisplay = GameManager.Instance.CurrentSaveData.GetInt("Floor " + mouseUIInteractable.floorNumber + "remainingItems");
	}
}
