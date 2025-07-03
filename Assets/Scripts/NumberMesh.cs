using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Displays a full number with several digits. View DigitMesh for more info.
/// Use caution as this will need to instantiate some objects when initializing.
/// </summary>
public class NumberMesh : MonoBehaviour {
	public int numberToDisplay = 1234;

	//Space between each character/digit
	[Header("Space between each digit")]
	public float characterSpacing = 0.5f;

	[Header("Each digit in the number is an instance of this prefab")]
	public DigitMesh digitMeshPrefab;

	private List<DigitMesh> digitMeshes;

	private int previousNumber;

	private void Awake(){
		digitMeshes = new List<DigitMesh>();

		previousNumber = numberToDisplay;

		DisplayNumber();
	}

	private void Update(){
		if(numberToDisplay != previousNumber){
			DisplayNumber();
		}

		previousNumber = numberToDisplay;
	}

	private void DisplayNumber(){
		string numberToString = Mathf.Abs(numberToDisplay).ToString();
		char[] numberToStringArray = numberToString.ToCharArray();
		int digitCount = numberToString.Length;

		//Ensure there are enough DigitMesh gameObjects instantiated
		while(digitMeshes.Count < digitCount){
			digitMeshes.Add(Instantiate(digitMeshPrefab));
		}

		//Place those same DigitMesh gameObjects in the correct locations.
		//By this I mean each DigitMesh will move one Digits width to the right until all digits have been placed in their correct position.
		for(int i = 0; i < digitMeshes.Count; i++){
			digitMeshes[i].transform.position = this.transform.position + ((-this.transform.right * characterSpacing) * i); //Positioning

			if(i < numberToString.Length){
				//Is within the count of digits for the number we're attempting to display
				digitMeshes[i].gameObject.SetActive(true);
				digitMeshes[i].digitToDisplay = int.Parse(numberToStringArray[i].ToString());
			} else {
				//Is NOT within the count of digits for the number we're attempting to display
				digitMeshes[i].gameObject.SetActive(false);
			}

			digitMeshes[i].transform.parent = null;

			digitMeshes[i].transform.rotation = this.transform.rotation;
			digitMeshes[i].transform.gameObject.layer = this.gameObject.layer;
			digitMeshes[i].transform.localScale = this.transform.localScale; //Multiply by 100 to adjust for blender stuff
			
			digitMeshes[i].transform.parent = this.transform;
			
			digitMeshes[i].UpdateMesh();
		}
	}
}
