using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsFileHandler : MonoBehaviour
{
    private const string OPTIONS_SAVE_PATH = "Options";

    public static SaveData optionsSaveData = new SaveData();

    private void Awake()
    {
        SetupOptionsSaveData();
    }

    private static void SetupOptionsSaveData()
    {
        optionsSaveData = SaveFileUtils.LoadSaveFile(OPTIONS_SAVE_PATH);

        if (optionsSaveData == null)
        {
            optionsSaveData = new SaveData();
        }
    }

    public static SaveData LoadOptions()
    {
        SetupOptionsSaveData();

        return optionsSaveData;
    }

    //Called through event in inspector on button pressed
    public static void SaveOptions()
    {
        UnityEngine.Debug.Log("Saving options");
        SaveFileUtils.WriteSaveToFile(OPTIONS_SAVE_PATH, optionsSaveData);
    }
}
