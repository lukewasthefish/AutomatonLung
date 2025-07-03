using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FramesPerSecondButton : CycleThroughOptionsButton
{
    private readonly int[] supportedFramerates = new int[]{ 30, 50, 60, 120, 144, 165, 240};

    private int desiredFPS = 60;

    public TextMeshPro textMesh;

    private void Start()
    {
        if(OptionsFileHandler.optionsSaveData.ContainsKey("FPS"))
            desiredFPS = OptionsFileHandler.optionsSaveData.GetInt("FPS");
    }

    private void Update()
    {
        textMesh.text = $"FPS:{desiredFPS}";
    }

    protected override int GetArraySize()
    {
        return supportedFramerates.Length;
    }

    protected override void Awake()
    {
        for (int i = 0; i < supportedFramerates.Length - 1; i++)
        {
            if (supportedFramerates[i] == GameManager.Instance.framerate)
            {
                currentArrayIndex = i;
            }
        }
    }

    protected override void ApplyArrayValue()
    {
        desiredFPS = supportedFramerates[currentArrayIndex];
        GameManager.Instance.framerate = desiredFPS;
        Application.targetFrameRate = GameManager.Instance.framerate;
        OptionsFileHandler.optionsSaveData.SetValue("FPS", desiredFPS);

        OptionsFileHandler.SaveOptions();
    }
}
