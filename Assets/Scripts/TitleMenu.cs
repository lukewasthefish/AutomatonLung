using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public string fileSelectScene;
    public string optionsScene;

    /// <summary>
    /// Enters file select menu scene
    /// </summary>
    public void StartGame()
    {
        GameManager.Instance.LoadSceneImmediate(fileSelectScene);
    }

    public void StartGame(Scene fileSelectScene)
    {
        GameManager.Instance.LoadSceneImmediate(fileSelectScene);
    }

    public void StartGame(string fileSelectScene)
    {
        GameManager.Instance.LoadSceneImmediate(fileSelectScene);
    }

    /// <summary>
    /// Goes to options menu
    /// </summary>
    public void Options(Scene optionsScene)
    {
        GameManager.Instance.LoadSceneImmediate(optionsScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
