using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SetCameraBackground : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Camera>().backgroundColor = Color.red;
    }
}
