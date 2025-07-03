using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTide : MonoBehaviour {

    public float magnitude = 0.25f;
    public float speed = 1.8f;

    private Vector3 startLocation;

    private float yPosition;
    private float sinePosition;

    private void Awake()
    {
        startLocation = transform.position;

        yPosition = startLocation.y;
    }

    private void LateUpdate()
    {
        sinePosition += speed * Time.deltaTime;

        yPosition = startLocation.y + 1 + Mathf.Sin(sinePosition)*magnitude;

        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }
}
