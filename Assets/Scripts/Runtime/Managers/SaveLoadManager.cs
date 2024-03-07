using System;
using Runtime.Utilities;
using UnityEngine;
using System.Threading.Tasks;

namespace Runtime.Managers
{
    public static class SaveDataValues
    {
        public static string Score => "Score";
        public static string Gold => "Gold";
        public static string Soul => "Soul";
        public static string TotalKill => "TotalKill";
        public static string LevelID => "LevelID";
        public static string OwnedVehicles => "OwnedVehicles";
        public static string OwnedWeapons => "OwnedWeapons";
    }
    
    public class SaveLoadManager : MonoBehaviour
    {
        public static SaveLoadManager Instance { get; private set; }
        private const string DEFAULT_PATH = "SaveFile.es3";

        private void Awake()
        {
            if (Instance is null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            
        }

        private void UnsubscribeEvents()
        {

        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void SaveData(string dataName, object value)
        {
            ES3.Save(dataName, value);
        }

        public void ArithmeticalData(string dataName, object value)
        {
            switch (value)
            {
                case int intValue:
                    var loadedInt = LoadData<int>(dataName);
                    ES3.Save(dataName, loadedInt + intValue);
                    break;
                case float floatValue:
                    var loadedFloat = LoadData<float>(dataName);
                    ES3.Save(dataName, loadedFloat + floatValue);
                    break;
                case byte byteValue:
                    var loadedByte = LoadData<byte>(dataName);
                    ES3.Save(dataName, loadedByte + byteValue);
                    break;
                default:
                    throw new ArgumentException("Unsupported type", nameof(value));
            }
            print(LoadData<object>(dataName).ColoredObj(Color.gray));
        }
        
        public T LoadData<T>(string dataName)
        {
            // if (ES3.FileExists(DEFAULT_PATH))
            // {
            //     if(ES3.KeyExists(dataName))
            //     {
            //         return ES3.Load<T>(dataName);
            //     }
            // }
            // return default(T);
            
            if (ES3.FileExists(DEFAULT_PATH))
            {
                if(ES3.KeyExists(dataName))
                {
                    return ES3.Load<T>(dataName);
                }
                else
                {
                    ES3.Save(dataName, default(T));
                }
            }
            return default(T);
        }
        
        public async Task<T> LoadDataAsync<T>(string dataName)
        {
            if (ES3.FileExists(DEFAULT_PATH))
            {
                if(ES3.KeyExists(dataName))
                {
                    // Assuming ES3.Load is the method that reads from the disk
                    return await Task.Run(() => ES3.Load<T>(dataName));
                }
                else
                {
                    await Task.Run(() => ES3.Save(dataName, default(T)));
                }
            }
            return default(T);
        }
        
        public void DeleteData(string dataName)
        {
            ES3.DeleteKey(dataName);
        }
    }
}