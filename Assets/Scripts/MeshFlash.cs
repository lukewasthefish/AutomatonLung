using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Blinks a Renderer on and off with the specified time variables
/// </summary>
public class MeshFlash : MonoBehaviour {

    public enum FlashType { DisableRenderer, TurnWhite }
    public FlashType flashType = FlashType.DisableRenderer;

    private Renderer thisRenderer;

    private Texture[] tempTex = null;

    public bool isColorRed = false;

    public bool isCurrentlyFlashing = false;

    private void Awake()
    {
        StopAllCoroutines();

        thisRenderer = this.GetComponent<Renderer>();

        if (thisRenderer == null)
        {
            Renderer[] renderersInChildren = GetComponentsInChildren<Renderer>(true);

            for(int i = 0; i < renderersInChildren.Length; i++){
                if(!renderersInChildren[i].GetComponent<TrailRenderer>()){
                    thisRenderer = renderersInChildren[i]; 
                }
            }
        }

        //Store texture(s) for FlashType.TurnWhite
        if (thisRenderer != null)
        {
            tempTex = new Texture[thisRenderer.materials.Length];
            for (int i = 0; i < tempTex.Length; i++)
            {
                tempTex[i] = thisRenderer.materials[i].mainTexture;
            }
        }

        if (this.GetComponent<Enemy>())
        {
            isColorRed = true;
        }

        if (originalShaderName == "")
        {
            originalShaderName = thisRenderer.material.shader.name;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetIsPaused())
        {
            SetBackToDefaultState();
            StopAllCoroutines();
        }
    }

    public void Flash(float secondsToFlash)
    {
        StopAllCoroutines();

        //if(!isCurrentlyFlashing)
        if(this.gameObject.activeSelf)
        StartCoroutine(FlashCoroutine(secondsToFlash));
    }

    private void OnDisable(){
        StopAllCoroutines();
    }

    private void SetBackToDefaultState()
    {
        thisRenderer.material.mainTexture = tempTex[0];
        thisRenderer.material.shader = Shader.Find(originalShaderName);
    }

    private string originalShaderName = "";
    IEnumerator FlashCoroutine(float secondsToFlash)
    {
        if (originalShaderName == "")
        {
            originalShaderName = thisRenderer.material.shader.name;
        }

        while (secondsToFlash > 0)
        {
            isCurrentlyFlashing = true;

            if (flashType == FlashType.DisableRenderer)
            {
                if (thisRenderer != null)
                {
                    thisRenderer.enabled = !thisRenderer.enabled;
                }
            }

            if(flashType == FlashType.TurnWhite)
            {
                if(thisRenderer.material.mainTexture == null)
                {
                    thisRenderer.material.mainTexture = tempTex[0];
                    thisRenderer.material.shader = Shader.Find(originalShaderName);
                } else
                {
                    thisRenderer.material.mainTexture = null;
                    thisRenderer.material.shader = Shader.Find("Unlit/Color");
                    if (isColorRed)
                    {
                        thisRenderer.material.color = Color.red;
                    }
                    else
                    {
                        thisRenderer.material.color = Color.white;
                    }
                }
            }

            secondsToFlash -= Time.deltaTime;
            yield return null;
        }

        if(thisRenderer != null)
        {
            thisRenderer.enabled = true;
            thisRenderer.material.shader = Shader.Find(originalShaderName);
            for (int i = 0; i < thisRenderer.materials.Length; i++)
            {
                thisRenderer.materials[i].mainTexture = tempTex[i];
            }
        }

        isCurrentlyFlashing = false;
    }

    public Renderer GetThisRenderer()
    {
        if (thisRenderer)
        {
            return thisRenderer;
        }

        thisRenderer = this.GetComponent<Renderer>();

        return thisRenderer;
    }
}
