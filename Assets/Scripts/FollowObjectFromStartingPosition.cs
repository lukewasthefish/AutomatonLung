using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DEPRECATED
/// </summary>
public class FollowObjectFromStartingPosition : MonoBehaviour {

    private Vector3 startPosition;
    [HideInInspector]public Vector3 startDistance; //Initial distance from objectToFollow

    public Transform objectToFollow;

    [HideInInspector]public Transform zoneCameraDestination;

    public bool includeY = false;
    public bool dontCopyObjectPosition = false;
    public bool includeRotation = false;
    public bool lookAtObject = false;
    public bool stayStill = false;
    public bool allowLerp = false;

    public float lerpSpeed = 1.5f;
    public float heightOverObject = 1f;

    private void Start()
    {
        startPosition = transform.position;

        //startDistance = objectToFollow.transform.position - startPosition;
        //transform.parent = null;
    }

    private void LateUpdate()
    {
        if(!objectToFollow){
            return;
        }

        if (!stayStill)
        {
            if (!includeY)
            {
                if (allowLerp)
                {
                    this.transform.position = Vector3.Lerp(transform.position, objectToFollow.transform.position - startDistance, lerpSpeed  * Time.deltaTime);
                } else
                {
                    this.transform.position = objectToFollow.transform.position - startDistance;
                }
            }
            else
            {
                //This script is bad
                if (!dontCopyObjectPosition && objectToFollow)
                {
                    transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y + heightOverObject, objectToFollow.transform.position.z);
                } else
                {
                    if(objectToFollow)
                    transform.position = new Vector3(objectToFollow.transform.position.x, 0f, objectToFollow.transform.position.z);
                }
            }
        }

        if (includeRotation)
        {
            transform.rotation = objectToFollow.transform.rotation;
        }

        if (lookAtObject)
        {
            transform.LookAt(objectToFollow);
        }

        if (!includeY && dontCopyObjectPosition)
        {
            //this.transform.position = Vector3.zero;
            transform.position = new Vector3(objectToFollow.transform.position.x, startPosition.y, objectToFollow.transform.position.z);
        }
    }
}
