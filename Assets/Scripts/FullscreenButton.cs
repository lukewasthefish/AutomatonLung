using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FullscreenButton : MonoBehaviour
{
    public TextMeshPro textMesh;

    private bool desiredEnabledStatus = true;

    private void Start()
    {
        if (OptionsFileHandler.optionsSaveData.ContainsKey("fullscreen"))
            desiredEnabledStatus = OptionsFileHandler.optionsSaveData.GetBool("fullscreen");
    }

    private void Update()
    {
        string offontext = desiredEnabledStatus == false ? "OFF" : "ON";

        textMesh.text = $"Fullscreen : {offontext}";
    }

    public void ToggleFullscreen()
    {
        if (desiredEnabledStatus == false)
        {
            desiredEnabledStatus = true;
        }
        else
        {
            desiredEnabledStatus = false;
        }

        OptionsFileHandler.optionsSaveData.SetValue("fullscreen", desiredEnabledStatus);

        GameManager.Instance.useOriginalResolution = OptionsFileHandler.optionsSaveData.GetBool("fullscreen");
        GameManager.Instance.Set3dsResolution();
    }
}
