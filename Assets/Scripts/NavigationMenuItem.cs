using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NavigationMenuItem : MonoBehaviour
{
    public abstract void MenuItemAction();

    [HideInInspector]
    public NavigationMenu navigationMenu;

    void OnMouseEnter()
    {
        // UnityEngine.Debug.Log("Mouse Enter " + this.gameObject.name);
        if (navigationMenu != null)
        {
            navigationMenu.SetFocus(this);
        }
    }
}
