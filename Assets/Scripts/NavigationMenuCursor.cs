using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMenuCursor : MonoBehaviour
{
    public NavigationMenu navigationMenu;

    public int initialFocus = 0;

    private ScrollCameraWithMouse scrollCameraWithMouse; //Optional; depends on Scene

    [HideInInspector]
    public bool activeScroll = true;

    private Transform previousParent = null;

    private void OnEnable()
    {
        if (!navigationMenu)
        {
            navigationMenu = FindObjectOfType<NavigationMenu>();
        }

        navigationMenu.SetFocus(initialFocus);
    }

    private void Awake()
    {
        scrollCameraWithMouse = FindObjectOfType<ScrollCameraWithMouse>(true);

        if(scrollCameraWithMouse)
        {
            scrollCameraWithMouse.navigationMenuCursor = this;
        }
    }

    private void Start()
    {
        if (!navigationMenu)
        {
            navigationMenu = FindObjectOfType<NavigationMenu>();
        }

        navigationMenu.SetFocus(initialFocus);
    }

    private void Update()
    {
        if (!navigationMenu)
        {
            navigationMenu = FindObjectOfType<NavigationMenu>();
        }

        if (!navigationMenu.GetFocus())
        {
            return;
        }

        Transform parent = navigationMenu.GetFocus().transform;

        this.transform.parent = parent;
        this.transform.localPosition = new Vector3(0f, 0f, -0.001f);
        this.transform.localScale = Vector3.one * 1.35f;

        if(this.transform.parent != previousParent)
        {
            this.activeScroll = true;
        }
        
        if(navigationMenu.GetFocus().TryGetComponent<CycleThroughOptionsButton>(out CycleThroughOptionsButton changeArrayValueButton))
        {
            if (GameManager.Instance.GetPlayerInputManager().GetMenuRightPressed())
            {
                changeArrayValueButton.IncrementArrayIndex();
            }

            if (GameManager.Instance.GetPlayerInputManager().GetMenuLeftPressed())
            {
                changeArrayValueButton.DecrementArrayIndex();
            }
        }

        if(scrollCameraWithMouse && activeScroll)
        {
            ScrollIfNeeded();
        }

        previousParent = this.transform.parent;
    }

    private void ScrollIfNeeded()
    {
        Vector3 nmcScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        // Debug.Log($"nmcScreenPos = {nmcScreenPos}");

        float screenHeightVal = Screen.height;

        if(GameManager.Instance.useOriginalResolution)
        {
            screenHeightVal = 240f;
        }

        if(nmcScreenPos.y >= screenHeightVal - scrollCameraWithMouse.GetDetectionThreshold() * 2f)
        {
            scrollCameraWithMouse.Scroll(scrollCameraWithMouse.transform.up);
        }

        if(nmcScreenPos.y <= scrollCameraWithMouse.GetDetectionThreshold() * 2f)
        {
            scrollCameraWithMouse.Scroll(-scrollCameraWithMouse.transform.up);
        }
    }
}
