using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;

    private Vector3 initialScale;
    private Vector3 referenceScale;

    private bool showingCrosshair = false;

    private void Awake()
    {
        initialScale = this.transform.localScale;
        referenceScale = initialScale;
        meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
    }

    private void Update()
    {
        referenceScale = Vector3.Lerp(referenceScale, initialScale, 10f * Time.deltaTime);

        this.transform.localScale = (referenceScale * 2f) + (Mathf.Cos(Time.time * 8f) * Vector3.one * 20f);
    }

    public void ShowCrosshair()
    {
        referenceScale = initialScale * 20f;

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if(meshRenderers[i])
            meshRenderers[i].enabled = true;
        }

        showingCrosshair = true;
    }

    public void HideCrosshair()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            if (meshRenderers[i])
                meshRenderers[i].enabled = false;
        }

        showingCrosshair = false;
    }

    public bool CrossHairShowing()
    {
        return showingCrosshair;
    }
}
