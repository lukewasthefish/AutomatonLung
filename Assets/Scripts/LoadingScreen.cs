using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using Debug = UnityEngine.Debug;

public class LoadingScreen : MonoBehaviour
{

    private static string sceneToLoad = "";

    private void Awake()
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        for(int i = 0; i < rootObjects.Length; i++){
            if (!rootObjects[i].GetComponent<GameManager>() && !rootObjects[i].GetComponent<Camera>() && rootObjects[i] != this.gameObject)
            {
                Destroy(rootObjects[i]);
            }
        }

        sceneToLoad = GameManager.Instance.destinationScene;
        
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
        System.GC.WaitForPendingFinalizers();

        StartCoroutine(GameManager.Instance.ChangeScene(sceneToLoad));
    }
}
