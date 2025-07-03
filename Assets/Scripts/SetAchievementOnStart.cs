using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAchievementOnStart : MonoBehaviour
{
    public string achievementToSet = "";

    private void Start()
    {
        if(SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.SetAchievement(achievementToSet);
            Steamworks.SteamUserStats.StoreStats();
        }
    }
}
