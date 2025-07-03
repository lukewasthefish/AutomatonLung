using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {

    public float timeUntilDestroy = 4f;

    private void Awake()
    {
        Invoke("DestroyThis", timeUntilDestroy);
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
