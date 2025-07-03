using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Collectible {

    [Header("Lower screen UI button corresponding to this weapon after unlock.")]
    public int weaponIndex = 0; //0 refers to the default unlocked weapon. 1-4 refer to the unlockable extra weaons.

    public AudioSource audioSource;

    protected override void CollectibleAction(ThirdPersonCharacterMovement thirdPersonCharacterMovement)
    {
        audioSource.transform.parent = null;
        audioSource.gameObject.SetActive(true);
        audioSource.Play();

        GameManager.Instance.weaponsUnlocked[weaponIndex] = true;

        string key = "weaponUnlocked" + weaponIndex;
        GameManager.Instance.CurrentSaveData.SetValue(key, true);

        GameManager.Instance.currentWeaponIndex = weaponIndex;

        int weaponsUnlockedCount = 0;
        for(int i = 0; i < GameManager.Instance.weaponsUnlocked.Length; i++)
        {
            if(GameManager.Instance.weaponsUnlocked[i] == true)
            {
                weaponsUnlockedCount++;
            }
        }

        if (weaponsUnlockedCount >= 5 && SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.SetAchievement("Achievement_AllWeapons");
            Steamworks.SteamUserStats.StoreStats();
        }
    }
}
