using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate1 : MonoBehaviour
{
    public float rotationSpeed;

    private void Awake()
    {
        InvokeRepeating("SlowUpdate", 0.2f, 0.1f);
    }

    private void SlowUpdate()
    {
        transform.Rotate(0, rotationSpeed * 50 * Time.deltaTime, 0, Space.Self);
    }
}
