using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Randomly and smoothly lerps the current light color among different values.
/// </summary>
public class ColorFluctuation : MonoBehaviour {

    [Header("Frequency at which to choose a new destination color for this light.")]
    public float newLightRepeatRate = 2f;

    private Light thisLight;

    private Vector3 colorValues;
    private Vector3 destinationColorValues;

    public float lerpSpeed = 0.25f;

    private void Awake()
    {
        thisLight = GetComponent<Light>();

        InvokeRepeating("ChangeColor", 0.1f, newLightRepeatRate);
    }

    private void Update()
    {
        thisLight.color = new Color( colorValues.x, colorValues.y, colorValues.z, thisLight.color.a);
        colorValues = Vector3.Lerp(colorValues, destinationColorValues, lerpSpeed * Time.deltaTime);
    }

    private void ChangeColor()
    {
        destinationColorValues = new Vector3(Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3));
        Debug.Log(destinationColorValues + "" + thisLight.color);
    }
}
