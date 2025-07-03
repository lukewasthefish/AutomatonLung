using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneImmediate : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        GameManager.Instance.LoadSceneImmediate(sceneName);
    }
}
