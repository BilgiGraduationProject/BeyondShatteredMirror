using System;
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

        [Space(20)]
        [Header("Gameplay Settings Panel")]
        [SerializeField] private GameObject gameplaySettingsPanel;
        [Space(10)]
        
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private TextMeshProUGUI sensitivityInfoText;
        
        [Space(20)]
        [Header("Camera Settings Panel")]
        [SerializeField] private GameObject cameraSettingsPanel;
        [Space(10)]
        
        [Space(20)]
        [Header("Video Settings Panel")]
        [SerializeField] private GameObject videoSettingsPanel;
        [Space(10)]
        
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [Space(10)]
        
        [SerializeField] private Toggle screenMode;
        [SerializeField] private TextMeshProUGUI screenModeInfoText;
        [Space(10)]
        
        [SerializeField] private Toggle vsyncToggle;
        
        [Space(20)]
        [Header("Audioplay Settings Panel")]
        [SerializeField] private GameObject audioSettingsPanel;
        [Space(10)]
        
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundVolumeSlider;
        
        #endregion

        #region Private Variables
        
        private Resolution[] resolutions;
        private List<Resolution> filteredResolutions;
        private float currentRefreshRate;
        private int currentResolutionIndex = 0;

        #endregion
        
        #endregion

        [Obsolete("Obsolete")]
        private void Awake()
        {
            Initialize();
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

        public void DeleteDatasClose()
        {
            gameplaySettingsPanel.SetActive(false);
            cameraSettingsPanel.SetActive(false);
            audioSettingsPanel.SetActive(false);
            videoSettingsPanel.SetActive(false);
            
            GameDataManager.ClearAllData();
            
            SetScreenMode(GameDataManager.LoadData<bool>(GameDataEnums.ScreenMode.ToString(), true));
            SetVSync(GameDataManager.LoadData<bool>(GameDataEnums.VSync.ToString(), true));
            SetMusicVolume(GameDataManager.LoadData<float>(GameDataEnums.MusicVolume.ToString(), 0.65f));
            SetSoundVolume(GameDataManager.LoadData<float>(GameDataEnums.SoundVolume.ToString(), 0.8f));
            SetSensitivity(GameDataManager.LoadData<float>(GameDataEnums.Sensitivity.ToString(), 1f));
            
            CoreUISignals.Instance.onClosePanel?.Invoke(2);
        }
        
        [Obsolete("Obsolete")]
        void InitResolution()
        {
            resolutions = Screen.resolutions;
            filteredResolutions = new List<Resolution>();
            
            resolutionDropdown.ClearOptions();
            currentRefreshRate = Screen.currentResolution.refreshRate;
            
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].refreshRate == currentRefreshRate)
                {
                    filteredResolutions.Add(resolutions[i]);
                }
            }
            
            List<string> options = new List<string>();
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
        
        [Obsolete("Obsolete")]
        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = filteredResolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
        }
        
        public void SetScreenMode(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            //Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            screenModeInfoText.text = isFullScreen ? "Fullscreen" : "Windowed";
        }
        
        public void SetVSync(bool isVSync)
        {
            QualitySettings.vSyncCount = isVSync ? 1 : 0;
        }
        
        public void SetMusicVolume(float volume)
        {
            CoreUISignals.Instance.onSetMusicVolume?.Invoke(volume);
        }
        
        public void SetSoundVolume(float volume)
        {
            CoreUISignals.Instance.onSetSoundVolume?.Invoke(volume);
        }
        
        public void SetSensitivity(float sensitivity)
        {
            // TODO: Change sensitivity using Signal
            sensitivityInfoText.text = sensitivity.ToString("F2");
        }
    }
}
