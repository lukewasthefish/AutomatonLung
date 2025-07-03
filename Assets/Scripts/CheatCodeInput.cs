using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CheatCodeInput : MonoBehaviour {

    private AudioSource audioSource;

    private bool canEnterCheatCodes = false;
    private bool cheatModeJustActivated = false;
    private bool conditionTriggeredAlready = false;

    // Cheat codes the player can use to make things happen in their game from the debug/cheat menu
    // To access the debug/cheat menu hold the hoverboard input and then press menu up. Please refer to the beginning of this class's Update() function for that code.
    // 0 = up
    // 1 = right
    // 2 = down
    // 3 = left
    private readonly List<int> flightMode = new List<int>{0,1,0,1,0,1}; //Up, Right, Up, Right, Up, Right
    private readonly List<int> allCollectibles = new List<int>{1,1,1,0,0,3}; //Right, Right, Right, Up, Up, Left
    private readonly List<int> invincible = new List<int> {0,3,0,3,0,3}; //Up, Left, Up, Left, Up, Left
    private readonly List<int> allWeapons = new List<int>{0,3,0,3,2,1}; //Up, Left, Up, Left, Down, Right
    private readonly List<int> allStandardCheats = new List<int>{1,3,3,2,0,1}; //Right, Left, Left, Down, Up, Right
    private readonly List<int> bobbleHead = new List<int>{0,0,3,1,0,2}; //Up, Up, Left, Right, Up, Down
    private readonly List<int> autocompleteMode = new List<int>{1,2,3,3,2,0}; //Right, Down, Left, Left, Down, Up
    private readonly List<int> debugRoom = new List<int> { 1, 2, 3, 0, 1, 2 };
    private readonly List<int> debugRoom1 = new List<int> { 0, 2, 3, 0, 1, 2 };
    private readonly List<int> debugRoom2 = new List<int> { 1, 2, 3, 0, 1, 3 };
    private readonly List<int> debugRoom3 = new List<int> { 1, 2, 3, 0, 1, 1 };
    private readonly List<int> debugRoom4 = new List<int> { 1, 2, 3, 1, 1, 1 }; 
    private readonly List<int> finalCutscene = new List<int>{2, 0, 2, 3, 2, 0}; //Down, Up, Down, Left, Down, Up
    private readonly List<int> enableFreecam = new List<int>{2,2,2,1,1,0}; //Down, Down, Down, Right, Right, Up

    //Current user created set of d-pad inputs
    private List<int> currentInputList = new List<int>{5,5,5,5,5,5};

    //In Awake initialize all the cheat codes with their values.
    //Cheat codes are entered on the d-pad. Up is 0, Right is 1, Down is 2, Left is 3.
    //Cheat codes are six values long each.
    //To unlock all crowns the player has to press up, up, up, down, left, down.
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //ENABLE DEBUG/CHEAT MENU
        if(GameManager.Instance.GetPlayerInputManager().GetMenuUpPressed() && GameManager.Instance.GetPlayerInputManager().GetCheatCodeModifierHeld())
        {
            if (!canEnterCheatCodes)
            {
                canEnterCheatCodes = true;
                cheatModeJustActivated = true;
            }
        }

        if (!GameManager.Instance.GetIsPaused() && FindObjectOfType<ThirdPersonCharacterMovement>() != null)
        {
            canEnterCheatCodes = false;
        }

        if (cheatModeJustActivated)
        {
            audioSource.Play();
            cheatModeJustActivated = false;
        }

        if (!canEnterCheatCodes)
        {
            currentInputList.Clear();
            return;
        }

        if (GameManager.Instance.GetPlayerInputManager().GetMenuUpPressed())
        {
            //UnityEngine.Debug.Log("UP");
            currentInputList.Add(0);
        }

        if (GameManager.Instance.GetPlayerInputManager().GetMenuRightPressed())
        {
            //UnityEngine.Debug.Log("RIGHT");
            currentInputList.Add(1);
        }

        if (GameManager.Instance.GetPlayerInputManager().GetMenuDownPressed())
        {
            //UnityEngine.Debug.Log("DOWN");
            currentInputList.Add(2);
        }

        if (GameManager.Instance.GetPlayerInputManager().GetMenuLeftPressed())
        {
            //UnityEngine.Debug.Log("LEFT");
            currentInputList.Add(3);
        }

        if (currentInputList.Count > 6)
        {
            currentInputList.RemoveAt(0);
        }

        //Test if currentInputList is equal to any of the lists defined in awake
        //First sorting the lists
        
        //Automaton Lung cheats
        if(!conditionTriggeredAlready && IsListSame(currentInputList, flightMode)){
            UnlockFlyMode();
        }

        if(!conditionTriggeredAlready && IsListSame(currentInputList, invincible)){
            UnlockInvincibility();
        }

        if(!conditionTriggeredAlready && IsListSame(currentInputList, allCollectibles)){
            UnlockAllCollectibles();
        }

        if(!conditionTriggeredAlready && IsListSame(currentInputList, allWeapons)){
            UnlockAllWeapons();
        }

        if(!conditionTriggeredAlready && IsListSame(currentInputList, bobbleHead)){
            EnableBobblehead();
        }

        if(!conditionTriggeredAlready && IsListSame(currentInputList, autocompleteMode)){
            EnableAutocompleteMode();
        }

        if (!conditionTriggeredAlready && IsListSame(currentInputList, debugRoom))
        {
            EnterDebugRoom();
        }
        if (!conditionTriggeredAlready && IsListSame(currentInputList, debugRoom1))
        {
            EnterDebugRoom1();
        }
        if (!conditionTriggeredAlready && IsListSame(currentInputList, debugRoom2))
        {
            EnterDebugRoom2();
        }
        if (!conditionTriggeredAlready && IsListSame(currentInputList, debugRoom3))
        {
            EnterDebugRoom3();
        }
        if (!conditionTriggeredAlready && IsListSame(currentInputList, debugRoom4))
        {
            EnterDebugRoom4();
        }
        if (!conditionTriggeredAlready && IsListSame(currentInputList, finalCutscene))
        {
            GoToFinalCutscene();
        }
        if (!conditionTriggeredAlready && IsListSame(currentInputList, enableFreecam))
        {
            EnableFreecam();
            prepareForAnotherPotentialCheatCode();
        }

        if (!conditionTriggeredAlready && IsListSame(currentInputList, allStandardCheats)){
            UnlockAllCollectibles();
            UnlockAllWeapons();
            UnlockFlyMode();
            UnlockInvincibility();
        }

        if (conditionTriggeredAlready)
        {
            prepareForAnotherPotentialCheatCode();
        }

        GameManager.Instance.canAchieveAchievements = !canEnterCheatCodes;
    }

    private void UnlockAllWeapons(){
        UnityEngine.Debug.Log("All weapons");

        for(int i = 0; i < GameManager.Instance.weaponsUnlocked.Length; i++){
            GameManager.Instance.CurrentSaveData.SetValue("weaponUnlocked" + i, true);
            GameManager.Instance.weaponsUnlocked[i] = true;
        }

        conditionTriggeredAlready = true;
    }

    private void UnlockAllClothing()
    {
        UnityEngine.Debug.Log("All clothing");
        //Cut for now

        //Maybe add fashion later?
    }

    private void UnlockAllCollectibles(){
        UnityEngine.Debug.Log("All collectibles");

        GameManager.Instance.chipCount = 999;
        GameManager.Instance.maxPlayerHealth = 100;
        GameManager.Instance.CurrentSaveData.SetValue("maxHealth", 100f);
        GameManager.Instance.CurrentSaveData.SetValue("ChipCount", 999);

        conditionTriggeredAlready = true;
    }

    private void EnableBobblehead(){
        UnityEngine.Debug.Log("Bobblehead");

        GameManager.Instance.bobbleHead = true;

        conditionTriggeredAlready = true;
    }

    private void UnlockFlyMode(){
        UnityEngine.Debug.Log("Flight mode unlocked.");

        GameManager.Instance.flightMode = true;

        conditionTriggeredAlready = true;
    }

    private void UnlockInvincibility(){
        UnityEngine.Debug.Log("Invincibility unlocked.");

        GameManager.Instance.invincible = true;

        conditionTriggeredAlready = true;
    }

    private void EnableAutocompleteMode(){
        UnityEngine.Debug.Log("Autocomplete mode unlocked");

        GameManager.Instance.autocompleteMode = true;

        conditionTriggeredAlready = true;
    }

    private void EnterDebugRoom()
    {
        GameManager.Instance.LoadSceneImmediate("DebugRoom");
    }
    private void EnterDebugRoom1()
    {
        GameManager.Instance.LoadSceneImmediate("DebugRoom 1");
    }
    private void EnterDebugRoom2()
    {
        GameManager.Instance.LoadSceneImmediate("DebugRoom 2");
    }
    private void EnterDebugRoom3()
    {
        GameManager.Instance.LoadSceneImmediate("DebugRoom 3");
    }
    private void EnterDebugRoom4()
    {
        GameManager.Instance.LoadSceneImmediate("DebugRoom 4");
    }

    private void GoToFinalCutscene()
    {
        GameManager.Instance.LoadSceneImmediate("FinalCutscene");
    }

    private void EnableFreecam()
    {
        FreeCameraMovement freecam = FindObjectOfType<FreeCameraMovement>(true);

        if(freecam != null)
        {
            GameManager.Instance.SetIsPaused(false);

            freecam.EnableFreecam();
        }
    }

    private void EnableFasterFlight()
    {
        UnityEngine.Debug.Log("Faster flight");

        GameManager.Instance.fasterFlight = true;

        conditionTriggeredAlready = true;
    }

    private void prepareForAnotherPotentialCheatCode()
    {
        currentInputList = new List<int> { 5, 5, 5, 5, 5, 5 };
        conditionTriggeredAlready = false;
    }

    private bool IsListSame(List<int> currentInput, List<int> ListToCompare)
    {
        if (currentInput == null || ListToCompare == null || currentInput.Count != ListToCompare.Count)
            return false;

        for(int x = 0; x < currentInput.Count; x++)
        {
            if(currentInput[x] != ListToCompare[x])
            {
                return false;
            }
        }

        UnityEngine.Debug.Log("Playing the audio source");
        audioSource.Play();
        return true;
    }
}
