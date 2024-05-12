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
                    GameDataManager.SaveData<int>(gameData.ToString(), tmpDropdown.value);
                    break;
                case Dropdown dropdown:
                    GameDataManager.SaveData<int>(gameData.ToString(), dropdown.value);
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
                    tmpDropdown.value = GameDataManager.LoadData<int>(gameData.ToString());
                    break;
                case Dropdown dropdown:
                    dropdown.value = GameDataManager.LoadData<int>(gameData.ToString());
                    break;
            }
        }
    }
}