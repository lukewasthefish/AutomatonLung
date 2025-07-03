using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShakeEffect : MonoBehaviour {

	private CameraShake camToShake;

	private void Start(){
		camToShake = GameManager.Instance.thirdPersonCharacterMovement.GetComponent<ThirdPersonCharacterMovement>().thirdPersonPlayerCamera.cameraShake;		
		InvokeRepeating("InitiatieShake", 1f, 1f);
	}

	private void InitiatieShake(){
		camToShake.Shake(Random.Range(0.1f,1f), Random.Range(0.1f, 12f));
	}
}
