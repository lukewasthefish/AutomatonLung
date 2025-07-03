using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionMenu : MonoBehaviour
{
    public static void SaveResolutionSettings()
    {
        GameManager.Instance.CurrentSaveData.SetValue("screenWidth", GameManager.Instance.DesiredScreenWidth);
        GameManager.Instance.CurrentSaveData.SetValue("screenHeight", GameManager.Instance.DesiredScreenHeight);
        GameManager.Instance.CurrentSaveData.SetValue("use3dsres", GameManager.Instance.useOriginalResolution);

        GameManager.Instance.Set3dsResolution();
        GameManager.Instance.ScaleResolution();
    }
}
