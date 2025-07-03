using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    private Vector2 leftStick;
    private Vector2 rightStick;

    private void Update()
    {
        //Testing buttons
        //bool pressed = GameManager.Instance.GetPlayerInputEvents().GetFirePressed();
        //bool held = GameManager.Instance.GetPlayerInputEvents().GetFireHeld();
        //bool released = GameManager.Instance.GetPlayerInputEvents().GetFireReleased();

        //UnityEngine.Debug.Log($"PRESSED : {pressed} HELD : {held} RELEASED : {released}");

        //Testing analog sticks
        //leftStick = GameManager.Instance.GetPlayerInputEvents().GetLeftStick();
        //rightStick = GameManager.Instance.GetPlayerInputEvents().GetRightStick();

        //UnityEngine.Debug.Log($"LEFTSTICK : {leftStick} RIGHTSTICK : {rightStick}");
    }
}
