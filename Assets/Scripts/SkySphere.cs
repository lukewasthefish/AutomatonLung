using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySphere : MonoBehaviour {

	private void Awake(){
		FollowObjectFromStartingPosition followObjectFromStartingPosition = this.GetComponent<FollowObjectFromStartingPosition>();
		ThirdPersonCharacterMovement tf = FindObjectOfType<ThirdPersonCharacterMovement>();

		if (followObjectFromStartingPosition && !followObjectFromStartingPosition.objectToFollow && tf)
		followObjectFromStartingPosition.objectToFollow = tf.transform;
	}

	private void Start(){
		ThirdPersonCharacterMovement tf = FindObjectOfType<ThirdPersonCharacterMovement>();

		if (tf)
		{
            //this.transform.localScale = Vector3.one * GameManager.Instance.GetRenderDistance() * 10f;
            this.transform.localScale = Vector3.one * 400f * 10f;
        }
	}
}
