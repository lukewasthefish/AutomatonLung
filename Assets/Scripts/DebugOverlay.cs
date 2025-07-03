using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugOverlay : MonoBehaviour {

    public Text debugText;

    private void Awake()
    {
        debugText = GetComponent<Text>();

        InvokeRepeating("FrameCheck", 0.1f, 0.1f);
    }

    private void FrameCheck()
    {
        //debugText.text = "Circle pad (X:" + playerReference.horizontal + "Y:" + playerReference.vertical + ")";
        debugText.text = "FPS:" + Mathf.RoundToInt( 1 / Time.deltaTime );
    }
}
