using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonitorSelectButton : CycleThroughOptionsButton
{
    private Display[] availableDisplays;

    public TextMeshPro textMesh;

    protected override void Awake()
    {
        availableDisplays = Display.displays;

        //Display.displays will always be of length 1 in the Editor corresponding to display 1.
        //Here we are faking a new array so that we can test with all 8 of the Unity Editor 'displays'
        #if UNITY_EDITOR
        availableDisplays = new Display[8];
        #endif   

        bool isOnSteamDeck = GameManager.Instance.IsOnSteamDeck();
        if(isOnSteamDeck)
        {
            PlayerPrefs.SetInt("UnitySelectMonitor", 0);
        }

        currentArrayIndex = PlayerPrefs.GetInt("UnitySelectMonitor");

        if(GameManager.Instance.desiredMonitor != -1)
        {
            currentArrayIndex = GameManager.Instance.desiredMonitor;
        }
    }

    private void Update()
    {
        textMesh.text = $"Active display ({currentArrayIndex+1}) (restart for this change to take effect)";
    }

    protected override void ApplyArrayValue()
    {
        ApplyChangesToPlayerPrefs();
    }

    public void ApplyChangesToPlayerPrefs()
    {
        GameManager.Instance.desiredMonitor = currentArrayIndex;
        PlayerPrefs.SetInt("UnitySelectMonitor", currentArrayIndex); //Surprise! We're using PlayerPrefs for this value. Look at the Unity discussions post on the next line for more info
        //https://discussions.unity.com/t/switch-monitor-during-runtime/124051
    }

    protected override int GetArraySize()
    {
        return availableDisplays.Length;
    }
}