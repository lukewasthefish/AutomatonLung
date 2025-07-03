using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnMouseClick : MonoBehaviour {

	public UnityEvent eventToTrigger;

	private Renderer thisRenderer;
	
	private void Awake(){
		thisRenderer = GetComponent<Renderer>();
	}

    private void OnMouseDown()
    {
		eventToTrigger.Invoke();

		thisRenderer.enabled = false;

		Invoke("EnableRenderer", 0.1f);
    }

	private void EnableRenderer(){
		thisRenderer.enabled = true;
	}
}
