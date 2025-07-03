using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lift on which the player may ride. Has functionality to move in any direction as desired by the designer.
/// </summary>
public class Lift : MonoBehaviour {
	public LiftWaypoint[] waypoints;

	public AudioClip liftSound;

	public bool isActive = true;

	public bool loop = true;

	[Header("True for this to parent player transform from any height")]
	public bool ignoreDistance = false;

	private AudioSource audioSource;

	private float currentSpeed;
	private float restTimeRemaining = 0f;
	private int currentWaypointIndex = 0;

	private bool isRaising = false; //Raise lift directly upwards

	private Quaternion initialRotation;

	private void Awake(){
		SetupAudio();
		initialRotation = this.transform.rotation;
		if(restTimeRemaining <= 0f){
			restTimeRemaining = 0.1f;
		}

		if(waypoints.Length > 0 && waypoints[0])
		this.transform.position = waypoints[0].transform.position;
	}

	private void SetupAudio(){
		audioSource = GetComponent<AudioSource>();

		if(!audioSource){
			audioSource = this.gameObject.AddComponent<AudioSource>();
		}

		audioSource.clip = liftSound;
		audioSource.loop = false;
		audioSource.playOnAwake = false;
		audioSource.spatialBlend = 1f;
		audioSource.rolloffMode = AudioRolloffMode.Linear;
		audioSource.maxDistance = 100f;
	}

	private void Update(){
		if(isRaising){
			this.transform.position += Vector3.up * 1f * Time.deltaTime;

			if(!audioSource.isPlaying){
				audioSource.Play();
			}
		}

		if(!isActive){
			return;
		}

		if(Vector3.Distance(this.transform.position, waypoints[currentWaypointIndex].transform.position) < 0.3f){
			ReachedWaypoint();
		} else if (restTimeRemaining > 0f){
			restTimeRemaining -= Time.deltaTime;
		} else {
			if(!audioSource.isPlaying){
				audioSource.Play();
			}
			this.transform.LookAt(waypoints[currentWaypointIndex].transform.position);
			this.transform.position += this.transform.forward * currentSpeed * Time.deltaTime;
			this.transform.rotation = initialRotation;
		}
	}

	public void ToggleActive(){
		isActive = !isActive;
	}

	public void RaiseUpward(){
		isRaising = true;
	}

	private void ReachedWaypoint(){
		this.transform.position = waypoints[currentWaypointIndex].transform.position;

		this.currentSpeed = waypoints[currentWaypointIndex].speedToNextWaypoint;
		this.restTimeRemaining = waypoints[currentWaypointIndex].RestTime;

		currentWaypointIndex++;

		//Loop index around if necessary
		if(currentWaypointIndex > waypoints.Length-1){
			currentWaypointIndex = 0;
			if(!loop){
				isActive = false;
			}
		}
	}
}
