using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingText : MonoBehaviour {

    public float movementSpeed = 0.5f;

    public float endOfLinePosition = 230f;

    private float startOfLinePosition;

    private void Awake()
    {
        startOfLinePosition = transform.position.x;
    }

    private void Update()
    {
        transform.position += new Vector3(-movementSpeed, 0f, 0f);

        if(transform.position.x < -endOfLinePosition)
        {
            transform.position = new Vector3(startOfLinePosition, transform.position.y, transform.position.z);
        }
    }
}
