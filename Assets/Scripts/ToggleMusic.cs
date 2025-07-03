using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMusic : MonoBehaviour
{
    public GameObject disabledIcon;

    private bool desiredMusicEnabled = true;

    private void Start()
    {
        desiredMusicEnabled = GameManager.Instance.musicEnabled;

        disabledIcon.SetActive(!desiredMusicEnabled);
    }

    public void Toggle()
    {
        desiredMusicEnabled = !desiredMusicEnabled;

        disabledIcon.SetActive(!desiredMusicEnabled);

        GameManager.Instance.musicEnabled = desiredMusicEnabled; //This is a special value because we want the effect of the music turning off previewed in the options menu

        OptionsFileHandler.optionsSaveData.SetValue("musicEnabled", desiredMusicEnabled);
        OptionsFileHandler.SaveOptions();
    }
}
