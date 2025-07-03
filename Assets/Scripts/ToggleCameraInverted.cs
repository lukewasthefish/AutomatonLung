using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCameraInverted : MonoBehaviour
{
    public Transform verticalArrowsDisplay;
    public Transform horizontalArrowsDisplay;

    private Vector3 initialVerticalArrowsDisplayScale;
    private Vector3 initialHorizontalArrowsDisplayScale;

    private bool desiredVerticalInverted = false;
    private bool desiredHorizontalInverted = false;

    private void Awake()
    {
        if(verticalArrowsDisplay)
        initialVerticalArrowsDisplayScale = verticalArrowsDisplay.transform.localScale;

        if(horizontalArrowsDisplay)
        initialHorizontalArrowsDisplayScale = horizontalArrowsDisplay.transform.localScale;

        desiredVerticalInverted = GameManager.Instance.invertCameraVertical;
        desiredHorizontalInverted = GameManager.Instance.invertCameraHorizontal;

        if (OptionsFileHandler.optionsSaveData.ContainsKey("invertVertical"))
            desiredVerticalInverted = OptionsFileHandler.optionsSaveData.GetBool("invertVertical");

        if (OptionsFileHandler.optionsSaveData.ContainsKey("invertHorizontal"))
            desiredHorizontalInverted = OptionsFileHandler.optionsSaveData.GetBool("invertHorizontal");
    }

    private void Update()
    {
        if (desiredVerticalInverted)
        {
            verticalArrowsDisplay.transform.localScale = new Vector3(initialVerticalArrowsDisplayScale.x, -initialVerticalArrowsDisplayScale.y, initialVerticalArrowsDisplayScale.z);
        }
        else
        {
            verticalArrowsDisplay.transform.localScale = new Vector3(initialVerticalArrowsDisplayScale.x, initialVerticalArrowsDisplayScale.y, initialVerticalArrowsDisplayScale.z);
        }

        if (desiredHorizontalInverted)
        {
            horizontalArrowsDisplay.transform.localScale = new Vector3(-initialHorizontalArrowsDisplayScale.x, initialHorizontalArrowsDisplayScale.y, initialHorizontalArrowsDisplayScale.z);
        }
        else
        {
            horizontalArrowsDisplay.transform.localScale = new Vector3(initialHorizontalArrowsDisplayScale.x, initialHorizontalArrowsDisplayScale.y, initialHorizontalArrowsDisplayScale.z);
        }
    }

    public void ToggleVerticalCameraInverted()
    {
        desiredVerticalInverted = !desiredVerticalInverted;

        GameManager.Instance.invertCameraVertical = desiredVerticalInverted;

        OptionsFileHandler.optionsSaveData.SetValue("invertVertical", desiredVerticalInverted);

        OptionsFileHandler.SaveOptions();
    }

    public void ToggleHorizontalCameraInverted()
    {
        desiredHorizontalInverted = !desiredHorizontalInverted;

        GameManager.Instance.invertCameraHorizontal = desiredHorizontalInverted;

        OptionsFileHandler.optionsSaveData.SetValue("invertHorizontal", desiredHorizontalInverted);
        
        OptionsFileHandler.SaveOptions();
    }
}
