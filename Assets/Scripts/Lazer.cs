using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Lazer : MonoBehaviour {
    [HideInInspector]public LineRenderer lineRenderer;

    [HideInInspector] public float lerpMultiplier = 1f;

    private AudioSource audioSource;

    private bool allowAudio = false;

    private bool canPlayAudio = true;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        audioSource = GetComponent<AudioSource>();
    }

    public void EnableAudio()
    {
        allowAudio = true;
    }

    private void LateUpdate()
    {
        if(allowAudio && lineRenderer.enabled && canPlayAudio)
        {
            audioSource.Play();

            canPlayAudio = false;
        }

        if (!lineRenderer.enabled)
        {
            audioSource.Stop();

            canPlayAudio = true;
        }

        lineRenderer.enabled = lineRenderer.widthMultiplier > 0.01f;

        if(lineRenderer && lineRenderer.enabled)
        {
            lineRenderer.widthMultiplier = Mathf.Lerp(lineRenderer.widthMultiplier, 0.005f, lerpMultiplier * Time.deltaTime);
        }
    }
}
