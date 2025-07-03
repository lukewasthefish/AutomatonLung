using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to fake crash the system. Now takes player to secret area.
/// </summary>
public class GameCrasher : MonoBehaviour {

	public GameObject spawnObject;
	public NumberMesh countDisplay;

    private void Update()
    {
		int distanceFromPlayerToStart = Mathf.RoundToInt(Vector3.Distance(Vector3.zero, GameManager.Instance.thirdPersonCharacterMovement.transform.position));

		if(distanceFromPlayerToStart > 999)
        {
			GameManager.Instance.LoadSceneImmediate("Harold 2");
			return;
        }

		countDisplay.numberToDisplay = distanceFromPlayerToStart;
    }
}
