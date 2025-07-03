using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoystick : MonoBehaviour {

    private const float maxDistanceFromStart = 12f;

    private Vector3 initialPosition;
    private Vector3 initialMousePosition; //When this is clicked, we need to store the location of the mouse when the click happened.
    private Vector3 vectorAwayFromCenter;

    private bool followingMouse = false;

    [HideInInspector] public float vertical;
    [HideInInspector] public float horizontal;

    private void Start()
    {
        initialPosition = this.transform.position;
    }

    private void OnMouseDown()
    {
        initialMousePosition = Input.mousePosition;
        followingMouse = true;
    }

    private void OnMouseUp()
    {
        followingMouse = false;
    }

    private void Update()
    {
        //Limit the maximum distance away from the joystick starting position
        vectorAwayFromCenter = (Input.mousePosition - initialMousePosition) / 60f;

        if (vectorAwayFromCenter.sqrMagnitude > maxDistanceFromStart)
        {
            //Head back to the initial position that this UIJoystick was positioned at on Awake()
            vectorAwayFromCenter /= vectorAwayFromCenter.sqrMagnitude * 6f * Time.deltaTime;
        }
        
        vertical = vectorAwayFromCenter.y / 2f; //Dividing by 2 is just an arbitrary value since by default the sizes for vertical and horizontal were too large.
        horizontal = vectorAwayFromCenter.x / 2f;

        if (followingMouse)
        {
            this.transform.position = initialPosition + vectorAwayFromCenter;
        } else
        {
            horizontal = 0f;
            vertical = 0f;
            vectorAwayFromCenter = Vector3.zero;
            this.transform.position = Vector3.Lerp(this.transform.position, this.initialPosition, 12f * Time.deltaTime);
        }
    }
}
