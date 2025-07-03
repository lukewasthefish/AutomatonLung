using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseSensitivityButton : CycleThroughOptionsButton
{
    private int[] supportedMouseSensitivities;

    private int desiredMouseSensitivity = 50; //Equilibrium

    public TextMeshPro textMesh;

    protected override void Awake()
    {
        supportedMouseSensitivities = new int[10];
        for (int i = 10; i < 110; i += 10)
        {
            supportedMouseSensitivities[(i/10)-1] = i;
        }

        for (int i = 0; i < supportedMouseSensitivities.Length; i++)
        {
            if (supportedMouseSensitivities[i] == GameManager.Instance.mouseSensitivity)
            {
                currentArrayIndex = i;
            }
        }

        GetArraySize();
    }

    private void Start()
    {
        if (OptionsFileHandler.optionsSaveData.ContainsKey("mouseSensitivity"))
            desiredMouseSensitivity = OptionsFileHandler.optionsSaveData.GetInt("mouseSensitivity");
    }

    private void Update()
    {
        textMesh.text = $"Mouse : {desiredMouseSensitivity}";
    }

    protected override void ApplyArrayValue()
    {
        desiredMouseSensitivity = supportedMouseSensitivities[currentArrayIndex];

        GameManager.Instance.mouseSensitivity = desiredMouseSensitivity;
        OptionsFileHandler.optionsSaveData.SetValue("mouseSensitivity", desiredMouseSensitivity);
        OptionsFileHandler.SaveOptions();
    }

    protected override int GetArraySize()
    {
        return supportedMouseSensitivities.Length;
    }
}
