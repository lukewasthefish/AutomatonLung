using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CrosshairBounds : MonoBehaviour
{
    public CrosshairMenuControls crosshairMenuControls;

    public float distanceAllowed = 2.5f;

    private void Update()
    {
        if (!crosshairMenuControls)
        {
            return;
        }

        while(Vector3.Distance(crosshairMenuControls.transform.position, this.transform.position) > distanceAllowed)
        {
            crosshairMenuControls.transform.position = Vector3.Lerp(crosshairMenuControls.transform.position, this.transform.position, 0.001f);
        }
    }
}
