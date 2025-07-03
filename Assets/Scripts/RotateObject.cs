using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed;

    public bool rotateX = false;
    public bool rotateY = false;
    public bool rotateZ = false;

    private int rotateXint = 0;
    private int rotateYint = 0;
    private int rotateZint = 0;

    private void Awake()
    {
        if (rotateX) rotateXint = 1;
        if (rotateY) rotateYint = 1;
        if (rotateZ) rotateZint = 1;
    }

    private void Update()
    {
        transform.Rotate(rotateXint * rotationSpeed * Time.deltaTime, rotateYint * rotationSpeed * Time.deltaTime, rotateZint * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
