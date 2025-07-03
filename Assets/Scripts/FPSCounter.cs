using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Debug; measures the framerate in FPS.
/// </summary>
public class FPSCounter : MonoBehaviour {

	private Text thisText;

    private int currentFPS;

    //public GameObject playerModel;

	private void Awake()
    {
        //playerModel.SetActive(false);
		thisText = GetComponent<Text>();
        InvokeRepeating("UpdateFPS", 10f, 0.5f);
    }

    private void Update()
    {
        currentFPS = Mathf.RoundToInt((1f / Time.unscaledDeltaTime));
    }

    private void UpdateFPS()
    {
        //currentFPS = Mathf.RoundToInt((1 / Time.unscaledDeltaTime));
        thisText.text = currentFPS.ToString();
        //playerModel.SetActive(true);
    }
}