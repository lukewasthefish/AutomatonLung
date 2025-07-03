using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour {
    // Scroll main texture based on time

    public float scrollSpeedX = 0.25f;
    public float scrollSpeedY = 0f;
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (scrollSpeedX <= 0) scrollSpeedX = 0.005f;
        if (scrollSpeedY <= 0) scrollSpeedY = 0.005f;

        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
