using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VsyncButton : MonoBehaviour
{
    public TextMeshPro textMesh;

    private int desiredVsyncValue = 0;

    private void Start()
    {
        if (OptionsFileHandler.optionsSaveData.ContainsKey("vSyncCount"))
            desiredVsyncValue = OptionsFileHandler.optionsSaveData.GetInt("vSyncCount");
    }

    private void Update()
    {
        string offontext = desiredVsyncValue == 0 ? "OFF" : "ON";

        textMesh.text = $"VSYNC : {offontext}";
    }

    public void ToggleVsync()
    {
        if(desiredVsyncValue == 0)
        {
            desiredVsyncValue = 1;
        } else
        {
            desiredVsyncValue = 0;
        }

        OptionsFileHandler.optionsSaveData.SetValue("vSyncCount", desiredVsyncValue);
        OptionsFileHandler.SaveOptions();
    }
}
