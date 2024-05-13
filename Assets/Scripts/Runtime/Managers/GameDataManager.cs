using UnityEngine;
using ES3Types;
using System.Collections.Generic;

namespace Runtime.Managers
{
    public static class GameDataManager
    {
        public static void SaveData<T>(string key, T value)
        {
            if (value is int intValue)
            {
                PlayerPrefs.SetInt(key, intValue);
            }
            else if (value is float floatValue)
            {
                PlayerPrefs.SetFloat(key, floatValue);
            }
            else if (value is string stringValue)
            {
                PlayerPrefs.SetString(key, stringValue);
            }
            else if (value is bool boolValue)
            {
                PlayerPrefs.SetInt(key, boolValue ? 1 : 0);
            }
            else if (value is byte byteValue)
            {
                PlayerPrefs.SetInt(key, byteValue);
            }
            else if (value is Resolution resolutionValue)
            {
                string resolutionString = resolutionValue.width + "x" + resolutionValue.height + "@" + resolutionValue.refreshRate;
                PlayerPrefs.SetString(key, resolutionString);
            }
            else
            {
                Debug.LogError("Type not supported");
            }
        }
        
        public static T LoadData<T>(string key, T defaultValue = default)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)PlayerPrefs.GetFloat(key, (float)(object)defaultValue);
            }
            else if (typeof(T) == typeof(string))
            {
                return (T)(object)PlayerPrefs.GetString(key, (string)(object)defaultValue);
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)(PlayerPrefs.GetInt(key, ((bool)(object)defaultValue) ? 1 : 0) == 1);
            }
            else if (typeof(T) == typeof(byte))
            {
                return (T)(object)(byte)PlayerPrefs.GetInt(key, (byte)(object)defaultValue);
            }
            else if (typeof(T) == typeof(Resolution))
            {
                string defaultVal = Screen.currentResolution.width + "x" + Screen.currentResolution.height + "@" + Screen.currentResolution.refreshRate;
                string resolutionString = PlayerPrefs.GetString(key, defaultVal);
                if (string.IsNullOrEmpty(resolutionString))
                {
                    return defaultValue;
                }
                string[] parts = resolutionString.Split('x', '@');
                Resolution resolution = new Resolution
                {
                    width = int.Parse(parts[0]),
                    height = int.Parse(parts[1]),
                    refreshRate = int.Parse(parts[2])
                };
                return (T)(object)resolution;
            }
            else
            {
                Debug.LogError("Type not supported");
                return defaultValue;
            }
        }

        public static void DeleteData(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }

        public static void ClearAllData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}