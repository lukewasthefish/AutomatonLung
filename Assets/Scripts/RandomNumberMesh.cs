using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NumberMesh))]
public class RandomNumberMesh : MonoBehaviour {
	private NumberMesh numberMesh;

	public float frequency = 0.1f;

	public int lowerBound = 1000;
	public int upperBound = 9999;

	private void Awake(){
		numberMesh = GetComponent<NumberMesh>();

		InvokeRepeating("NewNumber", 0f, frequency);
	}

	private void NewNumber(){
		numberMesh.numberToDisplay = Mathf.RoundToInt( Random.Range(lowerBound, upperBound) );
	}
}
