using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocaleButton : MonoBehaviour
{
    public string language = "English";

    /// <summary>
    /// Sets current language to the language associated with the button and saves the value in the options SaveData
    /// </summary>
    public void SetLanguage()
    {
        OptionsFileHandler.optionsSaveData.SetValue("language", language);
        
        OptionsFileHandler.SaveOptions();

        GameManager.Instance.SetLanguage();
    }
}
