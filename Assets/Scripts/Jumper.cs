using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spring / Trampoline the player can jump on to launch themselves incredibly high into the air
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Jumper : MonoBehaviour {

	private AudioSource audioSource;

	public float jumpStrength = 100f;

	public Transform movingPart;
	private Vector3 initialMovingPartPosition;
	private Vector3 destinationMovingPartPosition;

	private Collider thisCollider;

	private float timeSinceActivated = 0f;

	private void Awake(){
		thisCollider = GetComponent<Collider>();

		thisCollider.isTrigger = true;

		initialMovingPartPosition = movingPart.position;
		destinationMovingPartPosition = initialMovingPartPosition;

		audioSource = GetComponent<AudioSource>();
		audioSource.playOnAwake = false;
	}

	private void Update(){
		movingPart.position = Vector3.Lerp(movingPart.position, destinationMovingPartPosition, 10f * Time.deltaTime);

		timeSinceActivated += Time.deltaTime;

		if(timeSinceActivated > 0.4f)
		{
			destinationMovingPartPosition = initialMovingPartPosition;

			this.thisCollider.enabled = true;
		}
	}

	private void OnTriggerEnter(Collider other){
		timeSinceActivated = 0f;

		this.thisCollider.enabled = false;

		ThirdPersonCharacterMovement thirdPersonCharacterMovement = other.GetComponent<ThirdPersonCharacterMovement>();

		if(thirdPersonCharacterMovement != null){
			audioSource.Play();

			//Boot the player off their hoverboard if they attempt to bounce on a Jumper while riding a hoverboard.
			if(thirdPersonCharacterMovement.GetMovementType() == ThirdPersonCharacterMovement.MovementType.Hoverboard){
				thirdPersonCharacterMovement.hoverBoardPressed = true;
			}
			thirdPersonCharacterMovement.Jump(jumpStrength);
			destinationMovingPartPosition = initialMovingPartPosition + Vector3.up;
		}
	}
}
