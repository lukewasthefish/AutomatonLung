using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleThroughOptionsButton : MonoBehaviour
{
    Resolution[] supportedResolutions;
    Resolution previouslySelectedResolution;

    protected int currentArrayIndex = 0;

    bool lastDirection = true; //right or left along the resolution array

    protected virtual void Awake()
    {
        //All of this is related only to the resolution options so MAKE THIS AN ABSTRACT CLASS
        previouslySelectedResolution.height = 0;
        previouslySelectedResolution.width = 0;

        supportedResolutions = Screen.resolutions;

        bool isOnSteamDeck = SystemInfo.operatingSystem.Contains("SteamOS");
        if(isOnSteamDeck)
        {
            supportedResolutions = new Resolution[1];
            supportedResolutions[0] = new Resolution();
            supportedResolutions[0].width = 1280;
            supportedResolutions[0].height = 800;
        }

        for (int i = 0; i < supportedResolutions.Length - 1; i++)
        {
            if(supportedResolutions[i].width == GameManager.Instance.DesiredScreenWidth && supportedResolutions[i].height == GameManager.Instance.DesiredScreenHeight)
            {
                currentArrayIndex = i;
            }
        }
    }

    //Here because it is meant to be overriden. Another case for why this should have been abstract in the first place.
    protected virtual int GetArraySize()
    {
        return supportedResolutions.Length;
    }

    public void IncrementArrayIndex()
    {
        lastDirection = true;

        currentArrayIndex++;
        ConstrainIndex();
        ApplyArrayValue();
    }

    public void DecrementArrayIndex()
    {
        lastDirection = false;

        currentArrayIndex--;
        ConstrainIndex();
        ApplyArrayValue();
    }

    private void ConstrainIndex()
    {
        if (currentArrayIndex > GetArraySize() - 1)
        {
            currentArrayIndex = 0;
        }

        if(currentArrayIndex < 0)
        {
            currentArrayIndex = GetArraySize() - 1;
        }
    }

    private bool adjustingForDuplicates = false;
    protected virtual void ApplyArrayValue()
    {
        GameManager.Instance.DesiredScreenWidth = supportedResolutions[currentArrayIndex].width;
        GameManager.Instance.DesiredScreenHeight = supportedResolutions[currentArrayIndex].height;

        if(!adjustingForDuplicates)
            AdjustForDuplicates(lastDirection);

        previouslySelectedResolution = supportedResolutions[currentArrayIndex];

        ScreenResolutionMenu.SaveResolutionSettings();

        //UnityEngine.Debug.Log($"Width : {GameManager.Instance.DesiredScreenWidth} Height : {GameManager.Instance.DesiredScreenHeight}");
    }

    private void AdjustForDuplicates(bool direction) //true for right false for left
    {
        this.adjustingForDuplicates = true;

        //Skip some resolutions here
        //We need to skip forward if last pick was to the right and backward if last pick was to the left
        //If there's only one available resolution just pick that one and move on
        if (supportedResolutions.Length <= 1)
        {
            return;
        }

        //hahahahahahahahahaha
        int numberOfAttempts = 32;
        while(numberOfAttempts > 0)
        {
            //In this case the duplicates have ended; the spell has been broken; we are free from this curse
            if (!( supportedResolutions[currentArrayIndex].height == previouslySelectedResolution.height && supportedResolutions[currentArrayIndex].width == previouslySelectedResolution.width ))
            {
                break;
            }

            if (direction) //right
            {
                IncrementArrayIndex();
            }
            else //left
            {
                DecrementArrayIndex();
            }

            numberOfAttempts--;
        }

        this.adjustingForDuplicates = false;
    }
}
