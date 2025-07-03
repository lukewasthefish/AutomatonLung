using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZone : MonoBehaviour {

    private Transform zoneCamera;

    public bool lookAtPlayer = true;
    public bool moveAtOffsetWithPlayer = false;

    private void Awake()
    {
        //zoneCamera should always be first in the hierarchy
        zoneCamera = transform.GetChild(0).transform;

        if (zoneCamera.GetComponent<Camera>() != null)
        {
            zoneCamera.GetComponent<Camera>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            ThirdPersonPlayerCamera tppc = Camera.main.GetComponent<ThirdPersonPlayerCamera>();

            tppc.cameraMode = ThirdPersonPlayerCamera.CameraMode.standard;
        }
    }
}