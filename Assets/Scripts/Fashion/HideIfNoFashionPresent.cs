using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Hides objects that are meant for the fashion room ONLY
/// </summary>
public class HideIfNoFashionPresent : MonoBehaviour
{
    private void Awake()
    {
        FashionMenu fashionMenu = FindObjectOfType<FashionMenu>();

        if (!fashionMenu)
        {
            this.gameObject.SetActive(false);
        }
    }
}
