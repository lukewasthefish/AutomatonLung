using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Collectible {

    protected override void CollectibleAction(ThirdPersonCharacterMovement thirdPersonCharacterMovement)
    {
        Debug.LogError("WHAT ARE YOU DOING HERE!");
        // thirdPersonCharacterMovement.GetComponent<PlayerCombat>().IncreaseMaxHealth(3);
    }
}
