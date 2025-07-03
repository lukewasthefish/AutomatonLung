using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

[RequireComponent(typeof(NavigationMenu))]
public class LocaleCursorPosition : MonoBehaviour
{
    public GameObject localeCursor;
    private NavigationMenu navigationMenu;

    private void Awake()
    {
        navigationMenu = this.GetComponent<NavigationMenu>();
    }

    private void Start()
    {
        AssignCursorToPosition();
    }

    private void Update()
    {
        AssignCursorToPosition();
    }

    private void AssignCursorToPosition()
    {
        foreach(NavigationMenuItem item in navigationMenu.navigationMenuItems)
        {
            if(item.TryGetComponent<SetLocaleButton>(out SetLocaleButton setLocaleButton) && LocalizationSettings.SelectedLocale.name.Contains(setLocaleButton.language))
            {
                Debug.Log($"assigned cursor position to locale : {setLocaleButton.name}");
                localeCursor.transform.position = item.transform.position - (item.transform.forward/32f);
            }
        }
    }
}
