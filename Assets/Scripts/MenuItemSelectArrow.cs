using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuItemSelectArrow : MonoBehaviour
{
    public CycleThroughOptionsButton cycleThroughOptionsButton;

    [Header("If this is not a right button, it is a left button.")]
    public bool isRightButton = true;

    private bool mouseInBounds = false;

    private bool cooldownNeeded = false;

    private void Awake()
    {
        if(cycleThroughOptionsButton == null)
            cycleThroughOptionsButton = GetComponentInParent<CycleThroughOptionsButton>(true);
    }

    private void OnMouseEnter()
    {
        mouseInBounds = true;
    }

    private void OnMouseExit()
    {
        mouseInBounds = false;
    }

    private void Update()
    {
        if(GameManager.Instance.GetPlayerInputManager().GetConfirmReleased() && mouseInBounds)
        {
            if(isRightButton)
            {
                SelectRight();
            } else 
            {
                SelectLeft();
            }

            Invoke("ResetCoolDown",0.2f);
        }
    }

    public void SelectRight()
    {
        if(cooldownNeeded)
        {
            return;
        }
        Debug.Log("right");
        cycleThroughOptionsButton.IncrementArrayIndex();

        cooldownNeeded = true;
    }

    public void SelectLeft()
    {
        if(cooldownNeeded)
        {
            return;
        }
        Debug.Log("left");
        cycleThroughOptionsButton.DecrementArrayIndex();

        cooldownNeeded = true;
    }

    private void ResetCoolDown()
    {
        cooldownNeeded = false;
    }
}
