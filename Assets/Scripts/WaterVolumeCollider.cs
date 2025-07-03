using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class WaterVolumeCollider : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerStay(Collider other)
    {
        ThirdPersonCharacterMovement thirdPersonCharacterMovement = other.GetComponent<ThirdPersonCharacterMovement>();
        if ((thirdPersonCharacterMovement))
        {
            DoWaterSlowdown(thirdPersonCharacterMovement);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ThirdPersonCharacterMovement thirdPersonCharacterMovement = other.GetComponent<ThirdPersonCharacterMovement>();
        if ((thirdPersonCharacterMovement))
        {
            RestoreSpeed(thirdPersonCharacterMovement);
        }
    }

    private void DoWaterSlowdown(ThirdPersonCharacterMovement thirdPersonCharacterMovement)
    {
        if(thirdPersonCharacterMovement.GetMovementType() == ThirdPersonCharacterMovement.MovementType.Hoverboard)
        {
            thirdPersonCharacterMovement.hoverBoardPressed = true;
        }

        thirdPersonCharacterMovement.SetCurrentMaxSpeed(thirdPersonCharacterMovement.maxWaterWalkSpeed);
    }

    private void RestoreSpeed(ThirdPersonCharacterMovement thirdPersonCharacterMovement)
    {
        thirdPersonCharacterMovement.SetCurrentMaxSpeed(thirdPersonCharacterMovement.maxSpeed);
    }
}
