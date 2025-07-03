using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WornClothingInitializer : MonoBehaviour
{
    //One item from each category can be worn at once
    public WornFashionItem[] headgearItems;
    public WornFashionItem[] topItems;
    public WornFashionItem[] bottomItems;
    public WornFashionItem[] leftShoeItems;
    public WornFashionItem[] rightShoeItems;
    public WornFashionItem[] hoverboardItems;

    public Transform headgearBone;
    public Transform topBone;
    public Transform bottomBone;
    public Transform leftShoesBone;
    public Transform rightShoesBone;
    public Transform hoverboardBone; //Not really a bone but still functionally the same for these purposes

    private WornFashionItem currentHeadgear;
    private WornFashionItem currentTop;
    private WornFashionItem currentBottom;
    private WornFashionItem currentLeftShoe;
    private WornFashionItem currentRightShoe;
    private WornFashionItem currentHoverboard;

    private void Start()
    {
        UpdateShownClothingItems();
    }

    private void UpdateShownClothingItems()
    {
        currentHeadgear = FindWornFashionItemInCollection(headgearItems);
        currentTop = FindWornFashionItemInCollection(topItems);
        currentBottom = FindWornFashionItemInCollection(bottomItems);
        currentLeftShoe = FindWornFashionItemInCollection(leftShoeItems);
        currentRightShoe = FindWornFashionItemInCollection(rightShoeItems);
        currentHoverboard = FindWornFashionItemInCollection(hoverboardItems);

        JoinFashionItemPrefabToBone(currentHeadgear, headgearBone);
        JoinFashionItemPrefabToBone(currentTop, topBone);
        JoinFashionItemPrefabToBone(currentBottom, bottomBone);
        JoinFashionItemPrefabToBone(currentLeftShoe, leftShoesBone);
        JoinFashionItemPrefabToBone(currentRightShoe, rightShoesBone);
        JoinFashionItemPrefabToBone(currentHoverboard, hoverboardBone);
    }

    private WornFashionItem FindWornFashionItemInCollection(WornFashionItem[] wornFashionItems)
    {
        for(int i = 0; i < wornFashionItems.Length; i++)
        {
            WornFashionItem currentWornFashionItem = wornFashionItems[i];

            if (currentWornFashionItem) //TODO : Load save data and check against currently worn fashion
            {
                return currentWornFashionItem;
            }
        }

        return null;
    }

    private void JoinFashionItemPrefabToBone(WornFashionItem wornFashionItem, Transform bone)
    {
        if (!wornFashionItem)
        {
            return;
        }

        WornFashionItem instantiatedWornFashionItem = Instantiate(wornFashionItem);
        instantiatedWornFashionItem.transform.parent = bone;
        instantiatedWornFashionItem.transform.localPosition = Vector3.zero;
    }
}