using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.GetPlayerInputManager().GetPausePressed())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        GameManager.Instance.TogglePaused();
    }

    public void PauseGame()
    {
        GameManager.Instance.SetIsPaused(true);
    }

    public void Resume()
    {
        GameManager.Instance.SetIsPaused(false);
    }
}
