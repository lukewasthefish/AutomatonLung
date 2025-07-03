using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows this water to bob up and down on a sine wave.
/// </summary>
public class WaterBob : MonoBehaviour
{

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

        yPosition = startLocation.y + Mathf.Sin(sinePosition) * magnitude;

        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }
}
