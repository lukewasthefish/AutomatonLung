using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMenu : MonoBehaviour
{
    [Tooltip("Menu items in order from top (first selected) to bottom")]
    public NavigationMenuItem[] navigationMenuItems;
    private Vector3[] navigationMenuItemInitialScales;

    private NavigationMenuItem focus;
    private int focusIndex = 0;

    [Tooltip("Fills navigation menu items based on their height in the object hierarchy relative to one another.")]
    public bool AutoPopulateNavigationMenuItems = false;

    private void Awake()
    {
        if (AutoPopulateNavigationMenuItems)
        {
            Autopopulate();
        }

        focus = navigationMenuItems[focusIndex];

        navigationMenuItemInitialScales = new Vector3[navigationMenuItems.Length];
        for(int i = 0; i < navigationMenuItems.Length; i++)
        {
            navigationMenuItemInitialScales[i] = navigationMenuItems[i].transform.localScale;
        }
    }

    private void Update()
    {
        if(GameManager.Instance.GetPlayerInputManager().GetMenuUpPressed())
        {
            //UnityEngine.Debug.Log("menu up");
            NavigateUp();
        }

        if(GameManager.Instance.GetPlayerInputManager().GetMenuDownPressed())
        {
            //UnityEngine.Debug.Log("menu down");
            NavigateDown();
        }

        if(GameManager.Instance.GetPlayerInputManager().GetConfirmPressed())
        {
            if(this.GetFocus())
                this.GetFocus().MenuItemAction();
        }

        focus = navigationMenuItems[focusIndex];
        
        //Dynamic animations
        for(int i = 0; i < navigationMenuItems.Length; i++)
        {
            if(navigationMenuItems[i] != focus)
            {
                navigationMenuItems[i].transform.localScale = Vector3.Lerp(navigationMenuItems[i].transform.localScale, navigationMenuItemInitialScales[i], 15f * Time.deltaTime);
            }
            else
            {
                navigationMenuItems[i].transform.localScale = Vector3.Lerp(navigationMenuItems[i].transform.localScale, navigationMenuItemInitialScales[i] * 1.25f, 15f * Time.deltaTime);
            }
        }
    }

    public void NavigateUp()
    {
        focusIndex--;
        SetFocus(focusIndex);
    }

    public void NavigateDown()
    {
        focusIndex++;
        SetFocus(focusIndex);
    }

    public NavigationMenuItem GetFocus()
    {
        return focus;
    }

    public int GetFocusIndex()
    {
        return focusIndex;
    }

    public void SetFocus(int newFocusIndex)
    {
        focusIndex = newFocusIndex;

        if(focusIndex < 0)
        {
            focusIndex = navigationMenuItems.Length - 1;
        }

        if(focusIndex >= navigationMenuItems.Length)
        {
            focusIndex = 0;
        }
    }

    public void SetFocus(NavigationMenuItem newFocus)
    {
        for(int i = 0; i < navigationMenuItems.Length; i++)
        {
            if(navigationMenuItems[i] != null && navigationMenuItems[i] == newFocus)
            {
                SetFocus(i);
            }
        }
    }

    private void Autopopulate()
    {
        navigationMenuItems = this.GetComponentsInChildren<NavigationMenuItem>(true);

        int currentSaveFileIndex = 1;
        for(int i = 0; i < navigationMenuItems.Length; i++)
        {
            navigationMenuItems[i].navigationMenu = this;

            NumberMeshDisplaySaveFileIndex saveFileIndexDisplay = navigationMenuItems[i].GetComponentInChildren<NumberMeshDisplaySaveFileIndex>();
            
            if (saveFileIndexDisplay)
            {
                saveFileIndexDisplay.GetNumberMesh().numberToDisplay = currentSaveFileIndex;
                currentSaveFileIndex++;
            }
        }
    }
}
