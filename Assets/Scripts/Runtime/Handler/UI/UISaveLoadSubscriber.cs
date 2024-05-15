using Runtime.Enums;
using Runtime.Enums.UI;
using Runtime.Interfaces;
using Runtime.Keys;
using Runtime.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace Runtime.Handler.UI
{
    public class UISaveLoadSubscriber : MonoBehaviour, ISavableLoadable
    {
        #region Self Variables

        #region Serialized Variables

        //[EnumToggleButtons]
        //[SerializeField] private UIElementTypes uiElementTypes;
        //[SerializeField] private string key;
        [Required] [EnumToggleButtons]
        [SerializeField] private GameDataEnums gameData;
        
        #endregion

        #region Private Region
        
        private Component _component;

        #endregion
        
        #endregion

        private void Awake()
        {
            _component = GetComponent<Toggle>() ?? GetComponent<Slider>() 
                ?? (Component)GetComponent<TMP_Dropdown>();
        }

        private void Start()
        {
            Load();
        }

        private void OnDisable()
        {
            Save();
        }

        public void Save()
        {
            switch (_component)
            {
                case Toggle toggle:
                    GameDataManager.SaveData<bool>(gameData.ToString(), toggle.isOn);
                    break;
                case Slider slider:
                    GameDataManager.SaveData<float>(gameData.ToString(), slider.value);
                    break;
                case TMP_Dropdown tmpDropdown:
                    switch (gameData)
                    {
                        case GameDataEnums.Resolution:
                            string resolutionString = tmpDropdown.options[tmpDropdown.value].text;
                            GameDataManager.SaveData<string>(gameData.ToString(), resolutionString);
                            break;
                        case GameDataEnums.None:
                            break;
                        default:
                            GameDataManager.SaveData<int>(gameData.ToString(), tmpDropdown.value);
                            break;
                    }
                    break;
            }
        }

        public void Load()
        {
            switch (_component)
            {
                case Toggle toggle:
                    switch (gameData)
                    {
                        case GameDataEnums.ScreenMode:
                            toggle.isOn = GameDataManager.LoadData<bool>(gameData.ToString(), true);
                            break;
                        case GameDataEnums.VSync:
                            toggle.isOn = GameDataManager.LoadData<bool>(gameData.ToString(), true);
                            break;
                        case GameDataEnums.None:
                            break;
                        default:
                            toggle.isOn = GameDataManager.LoadData<bool>(gameData.ToString());
                            break;
                    }
                    break;
                case Slider slider:
                    switch (gameData)
                    {
                        case GameDataEnums.MusicVolume:
                            slider.value = GameDataManager.LoadData<float>(gameData.ToString(), .75f);
                            break;
                        case GameDataEnums.SoundVolume:
                            slider.value = GameDataManager.LoadData<float>(gameData.ToString(), .9f);
                            break;
                        case GameDataEnums.Sensitivity:
                            slider.value = GameDataManager.LoadData<float>(gameData.ToString(), 1f);
                            break;
                        case GameDataEnums.None:
                            break;
                        default:
                            slider.value = GameDataManager.LoadData<float>(gameData.ToString());
                            break;
                    }
                    break;
                case TMP_Dropdown tmpDropdown:
                    switch (gameData)
                    {
                        case GameDataEnums.Resolution:
                            string defaultResolution = Screen.currentResolution.width + " x " + Screen.currentResolution.height + " @ " + Screen.currentResolution.refreshRate + "Hz";
                            string resolutionString = GameDataManager.LoadData<string>(gameData.ToString(), defaultResolution);
                            int resolutionIndex = tmpDropdown.options.FindIndex(option => option.text == resolutionString);
                            if (resolutionIndex != -1)
                            {
                                tmpDropdown.value = resolutionIndex;
                            }
                            else
                            {
                                Debug.Log("Saved resolution not found in dropdown options");
                            }
                            break;
                        case GameDataEnums.None:
                            break;
                        default: tmpDropdown.value = GameDataManager.LoadData<int>(gameData.ToString());
                            break;
                    }
                    break;
            }
        }
    }
}