using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCutsceneCamera : MonoBehaviour
{
    public GameObject[] nodes;

    private void Update()
    {
        foreach(GameObject node in nodes)
        {
            if(node.activeSelf)
            {
                this.transform.SetPositionAndRotation(node.transform.position, node.transform.rotation);
            }
        }
    }
}
