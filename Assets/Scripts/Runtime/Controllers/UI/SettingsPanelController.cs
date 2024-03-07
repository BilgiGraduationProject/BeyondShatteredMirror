using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;
using UnityEngine.UI;
using Runtime.Managers;
using Runtime.Utilities;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using Runtime.Signals;

namespace Runtime.Controllers.UI
{
    public class SettingsPanelController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        [Header("Gameplay Settings Panel")]
        [SerializeField] private Toggle gameplay_settings1;
        [SerializeField] private TMP_Dropdown gameplay_settings2;
        [SerializeField] private Slider gameplay_settings3;
        
        #endregion

        #endregion

        private void Awake()
        {
            LoadAllSettings();
        }

        private async void LoadAllSettings()
        {
            gameplay_settings1.isOn = await SaveLoadManager.Instance.LoadDataAsync<int>("Settings1") == 1;
            //gameplay_settings2.value = await SaveLoadManager.Instance.LoadDataAsync<int>("Settings2");
            //gameplay_settings3.value = await SaveLoadManager.Instance.LoadDataAsync<float>("Settings3");
        }
        
        public void SaveAllSettings()
        {
            SaveLoadManager.Instance.SaveData("Settings1", gameplay_settings1.isOn ? 1 : 0);
            //SaveLoadManager.Instance.SaveData("Settings2", gameplay_settings2.value);
            //SaveLoadManager.Instance.SaveData("Settings3", gameplay_settings3.value);
        }
    }
}
