using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairMenuControls : MonoBehaviour
{
    public float speed = 100f;

    private void Update()
    {
        if (GameManager.Instance.GetPlayerInputManager().GetConfirmPressed() || GameManager.Instance.GetPlayerInputManager().GetJumpPressed())
        {
            Press(this.transform);
        }

        this.transform.localPosition += new Vector3(GameManager.Instance.GetPlayerInputManager().GetLeftStick().x * speed, GameManager.Instance.GetPlayerInputManager().GetLeftStick().y * speed, 0f ) * Time.deltaTime;
    }

    public static void Press(Transform cursorTransform)
    {
        RaycastHit hit;
        Physics.Linecast(cursorTransform.transform.position, cursorTransform.transform.position - (cursorTransform.transform.forward * 10f), out hit);
        Debug.DrawLine(cursorTransform.transform.position, cursorTransform.transform.position - (cursorTransform.transform.forward * 10f), Color.green, 4f);

        MouseUIInteractable mouseUIInteractable = null;

        if(hit.transform && hit.transform.GetComponent<MouseUIInteractable>())
        {
            mouseUIInteractable = hit.transform.GetComponent<MouseUIInteractable>();
        }

        if (hit.transform && mouseUIInteractable)
        {
            mouseUIInteractable.Press();

            if(mouseUIInteractable.actionType == MouseUIInteractable.ActionType.LoadScene)
            {
                Destroy( cursorTransform.transform.gameObject );
            }
        }
    }
}
