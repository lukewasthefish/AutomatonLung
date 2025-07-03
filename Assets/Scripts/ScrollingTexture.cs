using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    // Scroll main texture based on time
    public float scrollSpeedX = 0.25f;
    public float scrollSpeedY = 0f;
    Renderer rend;

    public bool adjustCoordinatesBasedOnTransform = false;

    public float debugScrollNumber = 100000f;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Mathf.Abs(scrollSpeedX) <= 0.0005) scrollSpeedX = 0.005f;
        if (Mathf.Abs(scrollSpeedY) <= 0.0005) scrollSpeedY = 0.005f;

        float offsetX = 0;
        float offsetY = 0;

        if (adjustCoordinatesBasedOnTransform)
        {
            offsetX = transform.position.x * debugScrollNumber;
            offsetY = transform.position.z * debugScrollNumber;
        }

        offsetX += Time.time * scrollSpeedX;
        offsetY += Time.time * -scrollSpeedY;

        if (Mathf.Abs(offsetX) > 2000f) { offsetX = 0f; }
        if (Mathf.Abs(offsetY) > 2000f) { offsetY = 0f; }

        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
