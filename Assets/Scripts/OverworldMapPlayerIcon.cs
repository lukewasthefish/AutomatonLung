using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldMapPlayerIcon : MonoBehaviour {

	public BeetleFlight beetleFlight;

	public GameObject arrowIcon; //Points in the direction for the player to go

	private Vector3 storedBeetleFlightPosition;

	private const float mapScale = 6100f;

	private void Awake()
    {
		if(beetleFlight == null)
			beetleFlight = FindObjectOfType<BeetleFlight>();
    }

	void Update () {
		if(beetleFlight.transform.position != storedBeetleFlightPosition && Vector3.Distance(beetleFlight.transform.position, storedBeetleFlightPosition) < 10f)
        {
			Vector3 distanceMoved = storedBeetleFlightPosition - beetleFlight.transform.position;
			distanceMoved /= mapScale;
			this.transform.position -= new Vector3(distanceMoved.x, distanceMoved.z, 0f);
        }

		storedBeetleFlightPosition = beetleFlight.transform.position;
	}
}
