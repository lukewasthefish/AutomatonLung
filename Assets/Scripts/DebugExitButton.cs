using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugExitButton : MonoBehaviour
{
    const string goodbyetext = "bye bye!";

    private void Update()
    {
        if (GameManager.Instance.GetPlayerInputManager().GetFirePressed())
        {
            foreach(TextMeshPro textmesh in FindObjectsOfType<TextMeshPro>(true))
            {
                textmesh.text = goodbyetext;
            }

            foreach(Renderer r in FindObjectsOfType<Renderer>(true))
            {
                if(!r.GetComponent<TextMeshPro>())
                    r.enabled = false;
            }

            Invoke("ExitGame", 2f);
        }
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
