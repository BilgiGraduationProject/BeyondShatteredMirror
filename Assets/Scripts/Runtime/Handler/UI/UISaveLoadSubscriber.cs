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
                ?? (Component)GetComponent<TMP_Dropdown>() ?? GetComponent<Dropdown>();
        }

        private void OnEnable()
        {
            switch (_component)
            {
                case Toggle toggle:
                    print("It is a toggle");
                    break;
                case Slider slider:
                    print("It is a slider");
                    break;
                case TMP_Dropdown tmpDropdown:
                    print("It is a TMP_Dropdown");
                    break;
                case Dropdown dropdown:
                    print("It is a Dropdown");
                    break;
            }
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
                    if (gameData == GameDataEnums.Resolution)
                    {
                        GameDataManager.SaveData<Resolution>(gameData.ToString(), 
                            GetResolutionFromDropdown(tmpDropdown));
                    }
                    else
                    {
                        GameDataManager.SaveData<int>(gameData.ToString(), tmpDropdown.value);
                    }
                    break;
                case Dropdown dropdown:
                    if (gameData == GameDataEnums.Resolution)
                    {
                        GameDataManager.SaveData<Resolution>(gameData.ToString(), 
                            GetResolutionFromDropdown(dropdown));
                    }
                    else
                    {
                        GameDataManager.SaveData<int>(gameData.ToString(), dropdown.value);
                    }
                    break;
            }
        }

        public void Load()
        {
            switch (_component)
            {
                case Toggle toggle:
                    toggle.isOn = GameDataManager.LoadData<bool>(gameData.ToString());
                    break;
                case Slider slider:
                    slider.value = GameDataManager.LoadData<float>(gameData.ToString());
                    break;
                case TMP_Dropdown tmpDropdown:
                    if (gameData == GameDataEnums.Resolution)
                    {
                        SetDropdownValueToResolution(tmpDropdown, gameData.ToString());
                    }
                    else
                    {
                        tmpDropdown.value = GameDataManager.LoadData<int>(gameData.ToString());
                    }
                    break;
                case Dropdown dropdown:
                    if (gameData == GameDataEnums.Resolution)
                    {
                        SetDropdownValueToResolution(dropdown, gameData.ToString());
                    }
                    else
                    {
                        dropdown.value = GameDataManager.LoadData<int>(gameData.ToString());
                    }
                    break;
            }
        }
        
        private Resolution GetResolutionFromDropdown(TMP_Dropdown tmpDropdown)
        {
            string[] parts = tmpDropdown.options[tmpDropdown.value].text.Split('x', '@');
            Resolution resolution = new Resolution
            {
                width = int.Parse(parts[0]),
                height = int.Parse(parts[1]),
                refreshRate = int.Parse(parts[2])
            };
            return resolution;
        }
        
        private Resolution GetResolutionFromDropdown(Dropdown dropdown)
        {
            string[] parts = dropdown.options[dropdown.value].text.Split('x', '@');
            Resolution resolution = new Resolution
            {
                width = int.Parse(parts[0]),
                height = int.Parse(parts[1]),
                refreshRate = int.Parse(parts[2])
            };
            return resolution;
        }
        
        private Resolution GetResolutionFromDropdownn(Component component)
        {
            string[] parts;
            if (component is TMP_Dropdown tmpDropdown)
            {
                parts = tmpDropdown.options[tmpDropdown.value].text.Split('x', '@');
            }
            else if (component is Dropdown dropdown)
            {
                parts = dropdown.options[dropdown.value].text.Split('x', '@');
            }
            else
            {
                return new Resolution();
            }

            Resolution resolution = new Resolution
            {
                width = int.Parse(parts[0]),
                height = int.Parse(parts[1]),
                refreshRate = int.Parse(parts[2])
            };
            return resolution;
        }
        
        private void SetDropdownValueToResolution(Component component, string key)
        {
            Resolution resolution = GameDataManager.LoadData<Resolution>(key,
                new Resolution
                {
                    width = Screen.currentResolution.width,
                    height = Screen.currentResolution.height,
                    refreshRate = Screen.currentResolution.refreshRate
                });
            string resolutionString = resolution.width + "x" + resolution.height + "@" + resolution.refreshRate;

            if (component is TMP_Dropdown tmpDropdown)
            {
                tmpDropdown.value = tmpDropdown.options.FindIndex(option => option.text == resolutionString);
            }
            else if (component is Dropdown dropdown)
            {
                dropdown.value = dropdown.options.FindIndex(option => option.text == resolutionString);
            }
        }
    }
}