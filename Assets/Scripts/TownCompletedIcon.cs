using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownCompletedIcon : MonoBehaviour {

	public NumberMeshTownChipCounter numberMeshTownChipCounter;

	private void Start(){
		string key = numberMeshTownChipCounter.townCharacter + "remainingThingsToCollect";
		if(GameManager.Instance.CurrentSaveData.ContainsKey(key) && GameManager.Instance.CurrentSaveData.GetInt(key) == 0)
        {
			return;
        }

		this.gameObject.SetActive(false);
	}
}
