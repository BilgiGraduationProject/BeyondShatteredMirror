using Runtime.Managers;
using UnityEngine;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Signals;
using TMPro;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class SettingsPanelController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        [Space(10)]
        [Header("Gameplay Settings Panel")]
        [SerializeField] private GameObject gameplaySettingsPanel;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        
        [Space(10)]
        [Header("Camera Settings Panel")]
        [SerializeField] private GameObject cameraSettingsPanel;
        
        
        [Space(10)]
        [Header("Video Settings Panel")]
        [SerializeField] private GameObject videoSettingsPanel;
        
        
        [Space(10)]
        [Header("Audioplay Settings Panel")]
        [SerializeField] private GameObject audioSettingsPanel;
        
        #endregion

        #region Private Variables
        
        private Resolution[] resolutions;
        private List<Resolution> filteredResolutions;
        private float currentRefreshRate;
        private int currentResolutionIndex = 0;

        #endregion
        
        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            InitResolution();
        }

        void Initialize()
        {
            gameplaySettingsPanel.SetActive(true);
            cameraSettingsPanel.SetActive(false);
            audioSettingsPanel.SetActive(false);
            videoSettingsPanel.SetActive(false);
        }

        public void ClosePanel()
        {
            CoreUISignals.Instance.onClosePanel?.Invoke(2);
        }
        
        void InitResolution()
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();
            
            resolutionDropdown.ClearOptions();
            currentRefreshRate = Screen.currentResolution.refreshRate;
            
            print($"Refresh Rate: {currentRefreshRate}");
            
            for (int i = 0; i < resolutions.Length; i++)
            {
                print($"Resolution: {resolutions[i]}");
                if (resolutions[i].refreshRate == currentRefreshRate)
                {
                    filteredResolutions.Add(resolutions[i]);
                }
            }
            
            List<string> options = new List<string>();
            print(GameDataEnums.Soul.ToString());
            for (int i = 0; i < filteredResolutions.Count; i++)
            {
                options.Add(filteredResolutions[i].width + " x " + filteredResolutions[i].height + " @ " + filteredResolutions[i].refreshRate + "Hz");
                if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
            
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
        
        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = filteredResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        }
    }
}
