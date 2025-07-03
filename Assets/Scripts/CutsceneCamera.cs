using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneCamera : MonoBehaviour
{

    [HideInInspector] public CutsceneManager cutsceneManager;

    public void switchCameras(int index)
    {
        cutsceneManager.activeCameraIndex = index;
    }
}
