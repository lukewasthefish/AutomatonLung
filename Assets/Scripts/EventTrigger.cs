using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class EventTrigger : MonoBehaviour {

	public UnityEvent eventToCall;

	private BoxCollider boxCollider;

	private void Awake(){
		boxCollider = GetComponent<BoxCollider>();

		boxCollider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other){
		if(other.GetComponent<ThirdPersonCharacterMovement>()){
			eventToCall.Invoke();

			this.gameObject.SetActive(false);
		}
	}
}
