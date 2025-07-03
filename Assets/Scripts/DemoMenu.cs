using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMenu : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.GetPlayerInputManager().GetConfirmPressed())
        {
            GameManager.Instance.LoadAfterDelay(0.1f, "TownD");
        }
    }
}
