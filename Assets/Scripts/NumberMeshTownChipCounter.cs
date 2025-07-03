using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NumberMesh))]
public class NumberMeshTownChipCounter : MonoBehaviour {

	public int defaultChipsInThisTown = 20;

	private NumberMesh numberMesh;

	[Header("Character in town name to load collected chips from.")]
	public char townCharacter = 'A';

	private void Awake(){
		numberMesh = GetComponent<NumberMesh>();
	}

	private void Start(){
		string key = townCharacter + "remainingThingsToCollect";
        if (!GameManager.Instance.CurrentSaveData.ContainsKey(key))
        {
            return;
        }

        numberMesh.numberToDisplay = GameManager.Instance.CurrentSaveData.GetInt(key);
    }
}
