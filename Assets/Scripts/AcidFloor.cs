using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AcidFloor : MonoBehaviour {

	public int damageOutput = 11;

	private BoxCollider boxCollider;

	private bool isDamageActive = false;

	private void Awake(){
		boxCollider = GetComponent<BoxCollider>();

		boxCollider.isTrigger = true;
		
		Invoke("Activate", 0.3f); //I think this was written to prevent cases where the player would spawn inside of an AcidFloor object and take unavoidabe damage
	}

	private void Activate(){
		isDamageActive = true;
	}

	private void OnTriggerStay(Collider other){
		if(!isDamageActive){
			return;
		}
		PlayerCombat playerCombat = null;
		if (other.GetComponent<ThirdPersonCharacterMovement>()){
			playerCombat = other.GetComponent<PlayerCombat>();

			if(!playerCombat.playerMeshFlash.isCurrentlyFlashing){
				playerCombat.TakeDamage(damageOutput, Vector3.zero);
			}
		}
	}
}
