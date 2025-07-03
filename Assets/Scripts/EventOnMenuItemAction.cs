using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventOnMenuItemAction : NavigationMenuItem
{
    public UnityEvent action;

    public override void MenuItemAction()
    {
        action.Invoke();
    }
}
