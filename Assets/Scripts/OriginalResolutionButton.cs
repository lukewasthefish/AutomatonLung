using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OriginalResolutionButton : MonoBehaviour
{
    public TextMeshPro textMesh;

    private bool desiredEnabledStatus = false;

    private void Start()
    {
        if (OptionsFileHandler.optionsSaveData.ContainsKey("use3dsres"))
            desiredEnabledStatus = OptionsFileHandler.optionsSaveData.GetBool("use3dsres");
    }

    private void Update()
    {
        string offontext = desiredEnabledStatus == false ? "OFF" : "ON";

        textMesh.text = $"Original Res:{offontext}";
    }

    public void ToggleUseOriginalResolution()
    {
        if (desiredEnabledStatus == false)
        {
            desiredEnabledStatus = true;
        }
        else
        {
            desiredEnabledStatus = false;
        }

        OptionsFileHandler.optionsSaveData.SetValue("use3dsres", desiredEnabledStatus);

        GameManager.Instance.useOriginalResolution = OptionsFileHandler.optionsSaveData.GetBool("use3dsres");
        GameManager.Instance.Set3dsResolution();
        
        ScreenResolutionMenu.SaveResolutionSettings();
        OptionsFileHandler.SaveOptions();
    }
}
