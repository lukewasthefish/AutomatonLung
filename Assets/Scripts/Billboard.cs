using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Always face the camera use Transform.LookAt(). Useful for 2D planes and sprites.
/// </summary>
public class Billboard : MonoBehaviour {

	Quaternion originalTransform;

	private void LateUpdate()
    {
        this.transform.LookAt(Camera.main.transform.position);
    }
}
