using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullThisObject : MonoBehaviour
{
    private Transform mainCameraTransform;

    public bool overrideGlobalCullingDistance = false;
    public float newCullingDistance = 200f;

    private float cullingDistance = 100f;

    private MeshRenderer meshRenderer;
    [HideInInspector] public bool cullingEnabled = true; //<- I am not sure why this has a [HideInInspector] tag. For now making anothr bool for editor use.
    public bool disableCulling = false;

    private void Awake()
    {
        if(disableCulling)
        {
            return;
        }

        meshRenderer = GetComponent<MeshRenderer>();

        if(meshRenderer != null)
        meshRenderer.enabled = false;

        mainCameraTransform = Camera.main.transform;

        if (overrideGlobalCullingDistance) cullingDistance = newCullingDistance;

        InvokeRepeating("SlowUpdate", 0f, 0.1f);
    }

    private void SlowUpdate()
    {
        if (!cullingEnabled || disableCulling)
        {
            //CancelInvoke("SlowUpdate");
            return;
        }

        if (Vector3.Distance(this.transform.position, mainCameraTransform.position) >= cullingDistance)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);

            if(meshRenderer != null)
            {
            meshRenderer.enabled = true;
            }
        }
    }
}
