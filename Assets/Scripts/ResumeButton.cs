using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : NavigationMenuItem
{
    public override void MenuItemAction()
    {
        GameManager.Instance.SetIsPaused(false);
    }
}
