using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour {

    public string sceneAfterCutscene = "HUB";

    public GameObject[] cameras;

    public int activeCameraIndex = 0;

    private void Start()
    {
        foreach(GameObject go in cameras)
        {
            CutsceneCamera cc = go.GetComponent<CutsceneCamera>();

            if(cc == null)
            {
                cc = go.AddComponent<CutsceneCamera>();
            }

            cc.cutsceneManager = this;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.GetPlayerInputManager().GetConfirmPressed())
        {
            if(SceneManager.GetActiveScene().name != "BossDeathCutscene")
            {
                EndCutscene();
            }
        }
    }

    private void LateUpdate()
    {
        for(int i = 0; i < cameras.Length; i++)
        {
            if(i == activeCameraIndex)
            {
                cameras[i].SetActive(true);
            } else
            {
                cameras[i].SetActive(false);
            }
        }
    }

    public void EndCutscene()
    {
        GameManager.Instance.destinationScene = sceneAfterCutscene;
        SceneManager.LoadSceneAsync("LoadingScene");
    }
}
