using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WindowedModeModifierButton : CycleThroughOptionsButton
{
    private FullScreenMode[] fullScreenModes = new FullScreenMode[0];

    protected override void Awake()
    {
        //The available FullScreenMode options will differ depending on the target platform.
        //See https://docs.unity3d.com/ScriptReference/FullScreenMode.html for more info
        #if UNITY_STANDALONE_WIN
        fullScreenModes = WindowsFullscreenModeOptions();
        #endif

        #if UNITY_STANDALONE_LINUX
        fullScreenModes = LinuxFullscreenModeOptions();
        #endif

        #if UNITY_STANDALONE_OSX
        fullScreenModes = MacFullscreenModeOptions();
        #endif

        if(SystemInfo.operatingSystem.Contains("SteamOS")) //On Steam Deck
        {
            fullScreenModes = new FullScreenMode[1];

            fullScreenModes[0] = GameManager.Instance.fullScreenMode;
        }


        foreach(FullScreenMode fsm in fullScreenModes)
        {
            UnityEngine.Debug.Log(fsm.ToString() + " is a window mode option.");
        }
        
        for (int i = 0; i < fullScreenModes.Length - 1; i++)
        {
            if(fullScreenModes[i] == GameManager.Instance.fullScreenMode)
            {
                currentArrayIndex = i;
            }
        }
    }

    protected override int GetArraySize()
    {
        return fullScreenModes.Length;
    }

    // private bool adjustingForDuplicates = false;
    protected override void ApplyArrayValue()
    {
        GameManager.Instance.fullScreenMode = fullScreenModes[currentArrayIndex];
        Screen.fullScreenMode = GameManager.Instance.fullScreenMode;
        OptionsFileHandler.optionsSaveData.SetValue("WindowMode", GameManager.Instance.fullScreenMode.ToString());

        GameManager.Instance.ScaleResolution();

        OptionsFileHandler.SaveOptions();
    }

    private FullScreenMode[] WindowsFullscreenModeOptions()
    {
        FullScreenMode[] windowsFSM = new FullScreenMode[3];

        windowsFSM[0] = FullScreenMode.ExclusiveFullScreen;
        windowsFSM[1] = FullScreenMode.FullScreenWindow;
        windowsFSM[2] = FullScreenMode.Windowed;

        return windowsFSM;
    }

    private FullScreenMode[] LinuxFullscreenModeOptions()
    {
        FullScreenMode[] linuxFSM = new FullScreenMode[2];

        linuxFSM[0] = FullScreenMode.FullScreenWindow;
        linuxFSM[1] = FullScreenMode.Windowed;

        return linuxFSM;
    }

    private FullScreenMode[] MacFullscreenModeOptions()
    {
        FullScreenMode[] macFSM = new FullScreenMode[3];

        macFSM[0] = FullScreenMode.FullScreenWindow;
        macFSM[1] = FullScreenMode.MaximizedWindow;
        macFSM[2] = FullScreenMode.Windowed;

        return macFSM;
    }
}
