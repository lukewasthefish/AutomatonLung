using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enable the Renderer component on this mesh on Start()
/// </summary>
public class EnableMeshRendererAfterAwake : MonoBehaviour {
    private Renderer thisRenderer;

    private void Awake()
    {
        thisRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        if (thisRenderer != null)
        {
            thisRenderer.enabled = true;
        }
    }
}
