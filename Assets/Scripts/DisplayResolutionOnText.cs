using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayResolutionOnText : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = this.GetComponent<TMP_Text>();
    }

    private void Update()
    {
        text.text = $"{GameManager.Instance.DesiredScreenWidth} x {GameManager.Instance.DesiredScreenHeight}";
    }
}
