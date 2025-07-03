using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FluctuateCameraBackgroundColor : MonoBehaviour {
	public Color destinationColor;

	public Light directionalLight;

	private Camera thisCamera;

	private void Awake(){
		thisCamera = GetComponent<Camera>();

		InvokeRepeating("UpdateColor", 0f, 0.5f);
	}

	private void Update(){
		thisCamera.backgroundColor = Color.Lerp(thisCamera.backgroundColor, destinationColor, 4f * Time.deltaTime);
	}

	private void UpdateColor(){
		float color1, color2, color3;
		color1 = Random.Range(0f, 1f);
		color2 = Random.Range(0f, 1f);
		color3 = Random.Range(0f, 1f);

		Color current = new Color(color1, color2, color3, 0f);
		destinationColor = current;

		if(directionalLight){
			directionalLight.color = current;
		}
	}
}
