using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorCompletedIcon : MonoBehaviour {

	private void InitializingFinished(){
		if(GameManager.Instance.CurrentSaveData.GetInt("Floor " + this.transform.parent.gameObject.GetComponent<MouseUIInteractable>().floorNumber + "remainingItems") != 0){
			this.gameObject.SetActive(false);
		}
	}
}
