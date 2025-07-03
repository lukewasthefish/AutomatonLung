using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ControlRebinder : MonoBehaviour
{
    public string actionName = "Jump";

    public InputActionAsset inputActionAsset;

    public GameObject[] objectsShowWhileBinding = new GameObject[0];

    public TMP_Text currentlyBoundControlsLabel;

    private InputActionMap gameplayMap;

    private InputAction inputActionToRebind;

    private void Awake()
    {
        gameplayMap = inputActionAsset.FindActionMap("Gameplay");
        inputActionToRebind = gameplayMap.FindAction(actionName);

        foreach (GameObject go in objectsShowWhileBinding)
        {
            go.SetActive(false);
        }

        UpdateLabelText();
    }

    private void Start()
    {
        UpdateLabelText();
    }

    private void Update()
    {
        UpdateLabelText();
    }

    private void UpdateLabelText()
    {
        if (inputActionToRebind == null)
        {
            return;
        }

        string labelDisplayString = "";

        for(int i = 0; i < inputActionToRebind.bindings.Count; i++)
        {
            if(inputActionToRebind != null && inputActionToRebind.bindings[i].path != null)
            {
                if (inputActionToRebind.bindings[i].path.Contains("Gamepad"))
                {
                    labelDisplayString += "Gamepad : ";
                }

                else if(inputActionToRebind.bindings[i].path.Contains("Keyboard"))
                {
                    labelDisplayString += "Keyboard : ";
                }

                else
                {
                    labelDisplayString += "Mouse : ";
                }
            }

            labelDisplayString += inputActionToRebind.bindings[i].ToDisplayString();

            if(i != inputActionToRebind.bindings.Count - 1)
            {
                labelDisplayString += "\n";
            }
        }

        currentlyBoundControlsLabel.text = labelDisplayString;
    }

    public void RebindAction()
    {
        UnityEngine.Debug.Log("Started rebinding");
        GameManager.Instance.GetPlayerInputManager().RebindAction(actionName, objectsShowWhileBinding);
    }

    public void RevertRebindToDefault()
    {
        SaveData loadedDefaultData = ControlBindingsFileSaver.LoadDefaultControlBindingsAndReturnValue();
        InputAction thisInputAction = gameplayMap.FindAction(this.actionName);

        ClearAllBindingsForAction();
        int numberOfBindingsForActionInDefault = 0;
        foreach(string key in loadedDefaultData.GetAllStrings().Keys)
        {
            string currentKeyWithoutIndex = key.Remove(key.Length - 1); //Remove index number
            if (currentKeyWithoutIndex.Equals(this.actionName)) //When the saved binding in loadedDefaultData is one of the bindings we need to add
            {
                thisInputAction.AddBinding(loadedDefaultData.GetString(key));
                numberOfBindingsForActionInDefault++;
            }
        }

        ControlBindingsFileSaver.SaveControlBindings();
    }
    
    public void ClearAllBindingsForAction()
    {
        GameManager.Instance.GetPlayerInputManager().ClearBindingsForAction(this.actionName);

        ControlBindingsFileSaver.SaveControlBindings();
    }
}
