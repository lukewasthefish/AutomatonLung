using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCountDoor : MonoBehaviour {

	public int chipsRequiredToPass = 10;

	[Header("Displays the chips required to pass value.")]
	public NumberMesh numberMesh;

	void Update () {
		numberMesh.numberToDisplay = chipsRequiredToPass;

		if(GameManager.Instance.chipCount >= chipsRequiredToPass){
			if(this.GetComponent<CullThisObject>()){
				this.GetComponent<CullThisObject>().cullingEnabled = false;
			}

			this.gameObject.SetActive(false);
		}
	}
}
