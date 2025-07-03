using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Removes parent transform; places this transform at the hierarchy root
/// </summary>
public class ClearParent : MonoBehaviour {

    private void Awake()
    {
        this.transform.SetParent(null);
    }

}
