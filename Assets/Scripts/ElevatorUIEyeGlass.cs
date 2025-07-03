using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUIEyeGlass : MonoBehaviour
{
    private void Update()
    {
        if(GameManager.Instance.GetPlayerInputManager().GetConfirmPressed() || GameManager.Instance.GetPlayerInputManager().GetFirePressed() || GameManager.Instance.GetPlayerInputManager().GetJumpPressed())
        {
            CrosshairMenuControls.Press(this.transform);
        }
    }
}
