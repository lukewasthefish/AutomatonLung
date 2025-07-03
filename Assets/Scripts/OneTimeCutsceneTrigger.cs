using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilizes the Collectible class to ensure that this cutscene is "collected" and therefore can only be triggered once
/// </summary>
public class OneTimeCutsceneTrigger : Collectible {

	public string cutsceneSceneName;

	[Header("Not really important for cutscenes; however it's here if you would like it")]
	public int destinationStart;

	protected override void CollectibleAction(ThirdPersonCharacterMovement thirdPersonCharacterMovement){
		GameManager.Instance.SetDestinationPlayerStart(this.destinationStart);
    	GameManager.Instance.LoadAfterDelay(0.3f, this.cutsceneSceneName);
	}

}
