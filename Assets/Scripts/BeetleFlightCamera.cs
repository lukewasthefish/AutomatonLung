using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The camera style used when following the player as they fly through the overworld.
/// </summary>
[RequireComponent(typeof(Camera))]
public class BeetleFlightCamera : MonoBehaviour {

    public Transform target; //What do we want to follow? (The player)

    public float distanceBehindBeetle = 7f;
    public float distanceAboveBeetle = 4f;

    public float minFOV = 70f;
    public float maxFOV = 90f;

    private float lowerMapOrthoSize;
    private float initialOrthoSize;

    public Camera lowerMapCamera;

    private Vector3 destinationPosition;

    private Camera thisCamera;
    private bool readyForLerp = false;

    private void Awake()
    {
        thisCamera = this.GetComponent<Camera>();
    }

    private void Start()
    {
        initialOrthoSize = lowerMapCamera.orthographicSize;
        lowerMapOrthoSize = initialOrthoSize;

        InitializePosition();
    }

    //So that we don't start zoomed extremely far from player
    private void InitializePosition()
    {
        destinationPosition = target.position - (target.forward * distanceBehindBeetle) + (target.up * distanceAboveBeetle);
        this.transform.position = destinationPosition;

        Invoke("EnableLerp", 0.2f);
    }

    private void EnableLerp()
    {
        readyForLerp = true;
    }

    private void LateUpdate()
    {
        if(!target) return;

        this.transform.LookAt(target);

        destinationPosition = target.position - (target.forward * distanceBehindBeetle) + (target.up * distanceAboveBeetle);

        if (readyForLerp)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, destinationPosition, 9f * Time.deltaTime);
        } else
        {
            this.transform.position = destinationPosition;
        }

        lowerMapCamera.orthographicSize = lowerMapOrthoSize;

        SetFOV();
    }

    private void SetFOV()
    {
        thisCamera.fieldOfView = (minFOV - 30f) + Vector3.Distance(this.transform.position, target.transform.position);
        thisCamera.fieldOfView = Mathf.Clamp(thisCamera.fieldOfView, minFOV, maxFOV);
    }
}
