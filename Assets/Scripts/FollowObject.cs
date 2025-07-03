using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DEPRECATED
/// </summary>
public class FollowObject : MonoBehaviour {

    public enum FollowType { Normal, XZOnly}
    public FollowType followType = FollowType.Normal;

    public Transform target;

    public Vector3 offset;

    public bool maintainStartingDistance = false;

    private Vector3 destination;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = this.transform.position;

        Debug.Log(this.gameObject.name + " is now following an object");
    }

    private void Update()
    {
        if(!target){
            return;
        }

        destination = !maintainStartingDistance ? target.position : initialPosition + target.position;

        if (followType == FollowType.Normal)
        {
            this.transform.position = destination + offset;
        }

        if(followType == FollowType.XZOnly)
        {
            this.transform.position = new Vector3(destination.x, this.transform.position.y, destination.z);
        }
    }
}
