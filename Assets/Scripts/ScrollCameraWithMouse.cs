using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrollCameraWithMouse : MonoBehaviour
{
    //Margins in which to start the vertical scrolling
    
    private float detectionThreshold = Screen.height / 10f;
    private const float SPEED = 6f;

    public float upperLimit = 100f;
    public float lowerLimit = 100f;

    private Vector3 upperLimitVector;
    private Vector3 lowerLimitVector;

    [HideInInspector]
    public NavigationMenuCursor navigationMenuCursor;

    private void Awake()
    {
        upperLimitVector = this.transform.position + (this.transform.up * upperLimit);
        lowerLimitVector = this.transform.position - (this.transform.up * lowerLimit);
    }

    private void LateUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;
        float screenHeight = Screen.height;

        // Check if the mouse is near the top of the screen.
        if (mousePosition.y >= screenHeight - detectionThreshold)
        {
            // Mouse is near the top of the screen.

            Scroll(this.transform.up);

            if(navigationMenuCursor)
                navigationMenuCursor.activeScroll = false;
        }

        // Check if the mouse is near the bottom of the screen.
        if (mousePosition.y <= detectionThreshold)
        {
            // Mouse is near the bottom of the screen.

            Scroll(-this.transform.up);

            if(navigationMenuCursor)
                navigationMenuCursor.activeScroll = false;
        }

        if(this.transform.position.y > upperLimitVector.y)
        {
            this.transform.position = upperLimitVector;
        }

        if(this.transform.position.y < lowerLimitVector.y)
        {
            this.transform.position = lowerLimitVector;
        }
    }

    public void Scroll(Vector3 direction)
    {
        //Double double, toil and trouble; Use this vector; Don't cause trouble.
        this.transform.position += SPEED * Time.deltaTime * direction;
    }

    public float GetDetectionThreshold()
    {
        return detectionThreshold;
    }
}
