using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Used in Elevator floor selection screen. Sets the remainingItems for each floor to a default value if there is not yet a key associated with that floor.
/// </summary>
public class ElevatorInitializer : MonoBehaviour {
	
	private const int collectiblesOnEachFloor = 5;

	public GameObject youAreHereIcon;
	private Vector3 youAreHereStartPosition;

	public Transform UICamera;

	public MouseUIInteractable[] mouseUIInteractables;

	private void Start(){
		youAreHereStartPosition = youAreHereIcon.transform.localPosition;
		youAreHereIcon.transform.parent = null;

		for(int i = -1; i < 22; i++){
			if(!GameManager.Instance.CurrentSaveData.ContainsKey("Floor " + i + "remainingItems")){
				GameManager.Instance.CurrentSaveData.SetValue("Floor " + i + "remainingItems", collectiblesOnEachFloor);
			}
		}

		BroadcastMessage("InitializingFinished");

		HandleYouAreHereIconPlacement();
	}

	private MouseUIInteractable currentFloorButton;
	private void HandleYouAreHereIconPlacement(){
		for(int i = 0; i < mouseUIInteractables.Length; i++){
			if(mouseUIInteractables[i].floorNumber == GameManager.Instance.previousElevatorFloorNumber){
				currentFloorButton = mouseUIInteractables[i];
				youAreHereIcon.transform.parent = mouseUIInteractables[i].transform;
				youAreHereIcon.transform.position = youAreHereStartPosition;
				break;
			}
		}

		youAreHereIcon.transform.parent = currentFloorButton.transform;
		youAreHereIcon.transform.localPosition = Vector3.zero;
		youAreHereIcon.transform.position += youAreHereIcon.transform.right * 12f;

		MoveCameraDownToYouAreHEreIcon();
	}

	private UIScroll uIScroll;
	private void MoveCameraDownToYouAreHEreIcon()
    {
        if (!uIScroll)
        {
			uIScroll = FindObjectOfType<UIScroll>();
        }

		while (youAreHereIcon.transform.position.y < UICamera.transform.position.y)
        {
			uIScroll.transform.position += Vector3.up / 1000f;
        }

		while (youAreHereIcon.transform.position.y > UICamera.transform.position.y)
		{
			uIScroll.transform.position += Vector3.down / 1000f;
		}
	}
}
