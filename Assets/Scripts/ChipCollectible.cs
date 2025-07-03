using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Collectible 'chips' are little trinkets hidden around the game. 
/// </summary>
public class ChipCollectible : Collectible {

    public AudioSource audioSource;
    
    protected override void Awake(){
        base.Awake();

        audioSource.playOnAwake = false;
    }

    protected override void CollectibleAction(ThirdPersonCharacterMovement thirdPersonCharacterMovement)
    {
        audioSource.transform.parent = null;
        audioSource.Play();

        PlayerCombat playerCombat = thirdPersonCharacterMovement.GetComponent<PlayerCombat>();

        if(!GameManager.Instance.CurrentSaveData.ContainsKey("ChipCount")){
            GameManager.Instance.CurrentSaveData.SetValue("ChipCount", 0);
        }

        if(!GameManager.Instance.CurrentSaveData.ContainsKey("maxHealth")){
            GameManager.Instance.CurrentSaveData.SetValue("maxHealth", playerCombat.maxHealth);
            playerCombat.currentHealth = playerCombat.maxHealth;
        }

        GameManager.Instance.chipCount++;

        if (SteamManager.Initialized && GameManager.Instance.chipCount >= 210)
        {
            Steamworks.SteamUserStats.SetAchievement("Achievement_AllChips");
            Steamworks.SteamUserStats.StoreStats();
        }

        playerCombat.IncrementMaxHealth();

        GameManager.Instance.CurrentSaveData.SetValue("ChipCount", GameManager.Instance.CurrentSaveData.GetInt("ChipCount") + 1);

        //If we're in a Town, since the Town may be made from a series of different Scenes, we should decrement remainingChips and create a new Save file variable if necessary.
        string sceneName = SceneManager.GetActiveScene().name;

        if(sceneName.Contains("Town")){
            char townLetter = sceneName[4]; //Towns are named like TownA, TownB, TownC, etc. so this will return the Town letter always. Sub-levels of towns are TownAsubareaname, TownBanothersubareaname, etc.
            string townLetertoString = townLetter.ToString();

            string key = townLetertoString + "remainingThingsToCollect";
            //UnityEngine.Debug.Log(key + " <- key at which to store the information for this town");
            if(!GameManager.Instance.CurrentSaveData.ContainsKey(key)){ //Create the key if it does not exist
                GameManager.Instance.CurrentSaveData.SetValue(key, 20); //There are 20 hidden items in each town. Hardcoded. Here we initialize that information to the key.
            }

            GameManager.Instance.CurrentSaveData.SetValue(key, GameManager.Instance.CurrentSaveData.GetInt(key) - 1); //Decrement remaining when collecting

            return;
        }

        //Elevator scene chip collectibles
        string elevatorKey = sceneName + "remainingItems";

        if(GameManager.Instance.CurrentSaveData.ContainsKey(elevatorKey))
        {
            GameManager.Instance.CurrentSaveData.SetValue(elevatorKey, GameManager.Instance.CurrentSaveData.GetInt(elevatorKey) - 1);
        } else {
            GameManager.Instance.CurrentSaveData.SetValue(elevatorKey, 4); //Initializing at 5 but also subtracting 1 to account for THIS chip collectible which will soonafter be removed from the scene.
        }
    }
}
