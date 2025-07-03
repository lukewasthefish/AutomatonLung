using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnPauseOnly : MonoBehaviour
{
    private List<Transform> objectsToShowOnPause = new List<Transform>();

    public enum ShowTime { PAUSED, NOT_PAUSED }
    public ShowTime showTime = ShowTime.PAUSED;

    private void Awake()
    {
        objectsToShowOnPause.AddRange(transform.gameObject.GetComponentsInChildren<Transform>());
    }

    private void Update()
    {
        bool showState = false;

        if(this.showTime == ShowTime.PAUSED && GameManager.Instance.GetIsPaused())
        {
            showState = true;
        }

        if(this.showTime == ShowTime.NOT_PAUSED && !GameManager.Instance.GetIsPaused())
        {
            showState = true;
        }

        SetEnabledStatusOfMenuElements(showState);
    }

    private void SetEnabledStatusOfMenuElements(bool value)
    {
        foreach(Transform transf in objectsToShowOnPause)
        {
            if(transf != this.transform)
            transf.gameObject.SetActive(value);
        }
    }
}
