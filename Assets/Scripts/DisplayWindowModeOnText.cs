using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayWindowModeOnText : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = this.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        text.text = $"Window mode : {GameManager.Instance.fullScreenMode.ToString()}";
    }
}
