using System.Collections.Generic;
using System;

[Serializable]
public class SaveData
{
    private Dictionary<string, bool> savedBools = new Dictionary<string, bool>();
    private Dictionary<string, int> savedInts = new Dictionary<string, int>();
    private Dictionary<string, float> savedFloats = new Dictionary<string, float>();
    private Dictionary<string, string> savedStrings = new Dictionary<string, string>();

    public SaveData()
    {

    }

    public SaveData(SaveData other)
    {
        this.savedBools = other.savedBools;
        this.savedInts = other.savedInts;
        this.savedFloats = other.savedFloats;
        this.savedStrings = other.savedStrings;
    }

    public void SetValue(string key, object value)
    {
        switch (value.GetType())
        {
            case Type boolType when boolType == typeof(bool):
                AddBoolValueToBeSaved(key, (bool)value);
                break;
            case Type intType when intType == typeof(int):
                AddIntValueToBeSaved(key, (int)value);
                break;
            case Type floatType when floatType == typeof(float):
                AddFloatValueToBeSaved(key, (float)value);
                break;
            case Type stringType when stringType == typeof(string):
                AddStringValueToBeSaved(key, (string)value);
                break;
        }
    }

    private void AddBoolValueToBeSaved(string key, bool value)
    {
        if (savedBools.ContainsKey(key))
        {
            savedBools[key] = value;
        } else
        {
            savedBools.Add(key, value);
        }
    }

    private void AddIntValueToBeSaved(string key, int value)
    {
        if (savedInts.ContainsKey(key))
        {
            savedInts[key] = value;
        }
        else
        {
            savedInts.Add(key, value);
        }
    }

    private void AddFloatValueToBeSaved(string key, float value)
    {
        if (savedFloats.ContainsKey(key))
        {
            savedFloats[key] = value;
        }
        else
        {
            savedFloats.Add(key, value);
        }
    }

    private void AddStringValueToBeSaved(string key, string value)
    {
        if (savedStrings.ContainsKey(key))
        {
            savedStrings[key] = value;
        }
        else
        {
            savedStrings.Add(key, value);
        }
    }

    public bool GetBool(string key)
    {
        if (!savedBools.ContainsKey(key))
        {
            SetValue(key, false);
        }

        return savedBools[key];
    }

    public int GetInt(string key)
    {
        if (!savedInts.ContainsKey(key))
        {
            SetValue(key, 0);
        }

        return savedInts[key];
    }

    public float GetFloat(string key)
    {
        if (!savedFloats.ContainsKey(key))
        {
            SetValue(key, 0f);
        }

        return savedFloats[key];
    }

    public string GetString(string key)
    {
        if (!savedStrings.ContainsKey(key))
        {
            SetValue(key, "");
        }

        return savedStrings[key];
    }

    public Dictionary<string, bool> GetAllBools()
    {
        return savedBools;
    }

    public Dictionary<string, int> GetAllInts()
    {
        return savedInts;
    }

    public Dictionary<string, float> GetAllFloats()
    {
        return savedFloats;
    }

    public Dictionary<string, string> GetAllStrings()
    {
        return savedStrings;
    }

    public bool ContainsKey(string key)
    {
        return savedBools.ContainsKey(key) || savedInts.ContainsKey(key) || savedFloats.ContainsKey(key) || savedStrings.ContainsKey(key);
    }

    public int NumberOfKeys()
    {
        return savedBools.Keys.Count + savedInts.Keys.Count + savedFloats.Keys.Count + savedStrings.Keys.Count;
    }
}
