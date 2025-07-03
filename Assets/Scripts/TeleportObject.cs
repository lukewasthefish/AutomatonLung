using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class TeleportObject : MonoBehaviour {

	public Transform destination;
	private SphereCollider sphereCollider;

	private void Awake(){
		sphereCollider = GetComponent<SphereCollider>();

		sphereCollider.isTrigger = true;

		foreach(TeleportObject teleportObject in GetComponentsInChildren<TeleportObject>())
        {
			teleportObject.transform.parent = null;
		}
	}

	private void OnTriggerEnter(Collider other){

		ThirdPersonCharacterMovement tpcm = other.GetComponent<ThirdPersonCharacterMovement>();
		if(tpcm && !tpcm.justTeleported){

			if (destination)
			{
				tpcm.transform.parent = null;
				tpcm.BeginTeleportTravel(destination);
			}
		}
	}
}
