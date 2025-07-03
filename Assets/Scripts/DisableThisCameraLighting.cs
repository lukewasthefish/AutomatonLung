using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DisableThisCameraLighting : MonoBehaviour {

    private Light[] allLights;

    private Color initialAmbientLightColor;

    private void Start()
    {
        allLights = FindObjectsOfType<Light>();

        initialAmbientLightColor = RenderSettings.ambientLight;
    }

    private void OnPreCull()
    {
        //disable lights
        for(int i = 0; i < allLights.Length; i++){
            if(allLights[i]){
                allLights[i].enabled = false;
            }
        }

        //Set ambient colors to black so the shadow doesn't show any color
        RenderSettings.ambientLight = Color.black;
    }

    private void OnPostRender()
    {
        //enable lights again
        for(int i = 0; i < allLights.Length; i++){
            if(allLights[i]){
                allLights[i].enabled = true;
            }
        }

        //Set ambient colors to their initial value
        RenderSettings.ambientLight = initialAmbientLightColor;
    }
}
