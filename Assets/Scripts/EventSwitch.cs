using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSwitch : MonoBehaviour {

	public UnityEvent eventToCall;

	//After one hit of the switch, should it hold in place? In other words can this EventSwitch be toggled multiple times.
	public bool hold = true;
	private bool hasBeenHitOnce = false;

	[Header("Visuals")]
	public GameObject sphere1;
	public GameObject sphere2;

	private SphereCollider sphereCollider;

	private AudioSource audioSource;

	private void Awake(){
		sphereCollider = GetComponent<SphereCollider>();
		audioSource = GetComponent<AudioSource>();

		sphereCollider.isTrigger = false;
	}

	// private void OnTriggerEnter(Collider other){
	// 	// Debug.Log("Something collided.");
	// 	// if(other.GetComponent<Bullet>() && (hold == false || !hasBeenHitOnce)){
	// 	// 	Debug.Log("It was a bullet.");
	// 	// 	ToggleCrystalState();
	// 	// 	eventToCall.Invoke();

	// 	// 	hasBeenHitOnce = true;
	// 	// }
	// }

	public void Trigger(){
		if(hold == false || !hasBeenHitOnce){
			ToggleCrystalState();
			eventToCall.Invoke();

			hasBeenHitOnce = true;
		}
	}

	/// <summary>
	/// Toggle the visual appearance of the crystal ball between the two different colors.
	/// </summary>
	private void ToggleCrystalState(){
		audioSource.Play();
		Debug.Log("The state was toggled.");
		sphere1.SetActive(!sphere1.activeSelf);
		sphere2.SetActive(!sphere2.activeSelf);
	}
}
