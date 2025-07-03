using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public enum MovementType { LOCAL, GLOBAL}
    public MovementType movementType = MovementType.LOCAL;

    public Vector3 movementPerSecond;

    private void Update()
    {
        this.transform.localPosition += movementPerSecond * Time.deltaTime;
    }
}
