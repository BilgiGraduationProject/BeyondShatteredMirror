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