using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FashionItemPickup : Collectible
{
    public int categoryIndex = 0;
    public int itemIndex = 0; //Which FashionItem of this category are we picking up

    protected override void CollectibleAction(ThirdPersonCharacterMovement thirdPersonCharacterMovement)
    {
        GameManager.Instance.CurrentSaveData.SetValue(SaveFileUtils.GetFashionItemUnlockedSaveKey(categoryIndex, itemIndex), true);
        GameManager.Instance.FashionUpdate();
    }
}
