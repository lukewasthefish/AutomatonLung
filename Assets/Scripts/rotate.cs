using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public float rotationSpeed;

    private void Awake()
    {
        InvokeRepeating("SlowUpdate", 0.1f, 0.15f);
    }

    private void SlowUpdate()
    {
        transform.Rotate(0, 0, rotationSpeed * 50 * Time.deltaTime, Space.Self);
    }
}
