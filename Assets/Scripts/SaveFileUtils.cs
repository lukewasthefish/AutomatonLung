using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class SaveFileUtils : MonoBehaviour
{
    /// <summary>
    /// Deletes the file associated with the provided filepath
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns>true if file deleted successfully</returns>
    public static bool DeleteSaveFile(string filePath)
    {
        string fileName = GetFileNameFromPath(filePath);
        string filePathWithName = Path.Combine(filePath, fileName);

        if (File.Exists(filePathWithName))
        {
            File.Delete(filePathWithName);

            //UnityEngine.Debug.Log("File deleted successfully");

            return true;
        }

        //UnityEngine.Debug.Log($"{filePathWithName} does not exist and is therefore not deleted");
        return false;
    }

    public static SaveData LoadSaveFile(string filePath)
    {
        string fileName = GetFileNameFromPath(filePath);

        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        string filePathWithName = Path.Combine(filePath, fileName);
        if (File.Exists(filePathWithName))
        {
            FileStream file = File.Open(filePathWithName, FileMode.Open);
            try{
                BinaryFormatter bf = new BinaryFormatter();
                SaveData loadedSaveData = (SaveData)bf.Deserialize(file);
                file.Close();
                //UnityEngine.Debug.Log("Loaded data from LoadSaveFile");
                return loadedSaveData;
            } catch (Exception e)
            {
                file.Close();
                UnityEngine.Debug.Log(e);
                return new SaveData();
            }

        }

        return null;
    }

    public static bool WriteSaveToFile(string filePath, SaveData saveDataToWrite)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        string fileName = GetFileNameFromPath(filePath);

        if(fileName == "")
        {
            UnityEngine.Debug.LogWarning("filename was empty. No save data written.");
            return false;
        }

        SaveData loadedData = LoadSaveFile(filePath);
        string filePathWithName = Path.Combine(filePath, fileName);
        if (loadedData != null)
        {
            file = File.Open(filePathWithName, FileMode.Open);
        } else
        {
            file = File.Create(filePathWithName);
        }

        bf.Serialize(file, saveDataToWrite);
        file.Close();
        Debug.Log("Save successful");
        return true;
    }

    public static string GetFileNameFromPath(string filePath)
    {
        if(filePath == "")
        {
            return "";
        }

        return "save" + filePath[filePath.Length - 1].ToString() + ".alungdata";
    }

    public static string GetFashionItemUnlockedSaveKey(int categoryIndex, int itemIndex)
    {
        return "clothingUnlocked" + categoryIndex.ToString() + "," + itemIndex.ToString();
    }

    public static string GetFashionItemWornSaveKey(int categoryIndex, int itemIndex)
    {
        return "clothingWorn" + categoryIndex.ToString() + "," + itemIndex.ToString();
    }
}
