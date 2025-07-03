using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAspectRatioIcon : MonoBehaviour
{
    Vector3 originalSize;

    private void Awake()
    {
        originalSize = this.transform.localScale;
    }

    private void Update()
    {
        float width = (GameManager.Instance.DesiredScreenWidth / 100f) * originalSize.x;
        float height = (GameManager.Instance.DesiredScreenHeight / 100f) * originalSize.y;

        this.transform.localScale = new Vector3(width, height, originalSize.z) / 8f;
    }
}
