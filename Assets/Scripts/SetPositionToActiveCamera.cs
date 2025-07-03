using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionToActiveCamera : MonoBehaviour {

	[Header("Find camera every frame and set position to that camera?")]
	public bool doOnUpdate = true;

	void Start () {
		if(Camera.current.transform != null)
			this.transform.position = Camera.current.transform.position;
	}

	void Update () {

        if (doOnUpdate && Camera.current.transform != null)
        {
			this.transform.position = Camera.current.transform.position;
		} 
	}
}
