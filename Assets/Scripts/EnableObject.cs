using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObject : MonoBehaviour
{
    public bool objectStartEnableState = false;

    private void Awake()
    {
        this.gameObject.SetActive(objectStartEnableState);
    }

    public void EnableForTimePeriod(float time)
    {
        this.gameObject.SetActive(true);

        Invoke("Disable", time);
    }

    private void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
