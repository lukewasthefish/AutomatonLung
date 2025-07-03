using UnityEngine;

public class FileSelectMenu : MonoBehaviour
{
    public string titleScene = "TitleMenu";

    public void ReturnToTitle()
    {
        GameManager.Instance.LoadSceneImmediate(titleScene);
    }

    public void LoadSaveFile(int fileIndex)
    {
        GameManager.Instance.CurrentSaveFileIndex = fileIndex;

        GameManager.Instance.LoadSaveData();

        if (!GameManager.Instance.CurrentSaveData.ContainsKey("saveScene"))
        {
            GameManager.Instance.CurrentSaveData.SetValue("saveScene", "Floor 21");
        }

        GameManager.Instance.LoadSceneImmediate(GameManager.Instance.CurrentSaveData.GetString("saveScene"));
    }
}
