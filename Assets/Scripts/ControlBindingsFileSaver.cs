using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles saving and loading of Control Binding preferences including but not limited to the default save file in such a case where no user-defined control bindings exist.
/// </summary>
public class ControlBindingsFileSaver : MonoBehaviour
{
    private const string controlBindingsPath = "ControlBindings/H/E/L/L/O"; //Location where the custom control binding is stored

    private void Start()
    {
        SaveData loadedControlBindings = LoadControlBindings();

        //In the case where there are not previously assigned control bindings load the default file
        if(loadedControlBindings == null)
        {
            loadedControlBindings = LoadDefaultControlBindingsAndReturnValue();
        }

        if(loadedControlBindings == null)
        {
            UnityEngine.Debug.LogError("Missing default control bindings file!");
        } else
        {
            ApplyLoadedData(loadedControlBindings);
        }
    }

    public static void SaveControlBindings()
    {
        SaveData saveDataToWrite = new SaveData();

        foreach (ControlRebinder controlRebinder in FindObjectsOfType<ControlRebinder>(true))
        {
            InputAction currentInputAction = GameManager.Instance.GetPlayerInputManager().GetInputActionAsset().FindActionMap("Gameplay").FindAction(controlRebinder.actionName);
            for (int i = 0; i < currentInputAction.bindings.Count; i++)
            {
                if(currentInputAction.bindings[i] != null)
                    saveDataToWrite.SetValue(controlRebinder.actionName + i, currentInputAction.bindings[i].effectivePath);
            }
        }

        SaveFileUtils.WriteSaveToFile(controlBindingsPath, saveDataToWrite);
    }

    public static SaveData LoadControlBindings()
    {
        SaveData loadedData = SaveFileUtils.LoadSaveFile(controlBindingsPath);

        return loadedData;
    }

    public static void ApplyLoadedData(SaveData loadedData)
    {
        if (loadedData == null)
        {
            return;
        }

        //Clearing existing bindings
        Dictionary<string, string> allLoadedStrings = loadedData.GetAllStrings();
        foreach (string key in allLoadedStrings.Keys)
        {
            string loadedActionName = key.Remove(key.Length - 1); //Removes the index from the end of the action name. i.e, Jump0 becomes Jump
            
            if(loadedActionName != "Confirm") //Lets not have anyone break their menus
                GameManager.Instance.GetPlayerInputManager().ClearBindingsForAction(loadedActionName);
        }

        foreach (string key in loadedData.GetAllStrings().Keys)
        {
            string loadedActionName = key.Remove(key.Length - 1);
            InputAction actionToRebindFromLoad = GameManager.Instance.GetPlayerInputManager().GetInputActionAsset().FindActionMap("Gameplay").FindAction(loadedActionName);

            //Will be gettings things like Fire0, Jump2, etc.
            //This loads the desired action name without the number we added to the end.
            //The number on the end is for the save data writing so that the actions can have several different bindings.
            actionToRebindFromLoad.AddBinding(loadedData.GetString(key));
        }
    }

    /// <summary>
    /// Loads and applies the default control bindings for all controls
    /// </summary>
    public void LoadDefaultControlBindings()
    {
        ApplyLoadedData(LoadDefaultControlBindingsAndReturnValue());
    }

    public static SaveData LoadDefaultControlBindingsAndReturnValue()
    {
        TextAsset defaultBindingsText = Resources.Load<TextAsset>("Bindings/defaultControlBindings");

        using (var stream = new MemoryStream(defaultBindingsText.bytes))
        {
            var formatter = new BinaryFormatter();
            var loadedSaveData = (SaveData)formatter.Deserialize(stream);
            return loadedSaveData;
        }
    }
}
