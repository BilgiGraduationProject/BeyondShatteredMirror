using System;
using Runtime.Enums.GameManager;
using Runtime.Signals;
using UnityEngine;
using Runtime.Enums.UI;
using UnityEngine.Playables;
using Runtime.Enums.Playable;
using Runtime.Enums.Pool;

namespace Runtime.Managers
{
    public class UIManager : MonoBehaviour
    {
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            
        }

        private void UnSubscribeEvents()
        {
            
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        public void OnStart()
        {
            // TODO: Add more shows to beatify the game start.
            CoreUISignals.Instance.onCloseAllPanels?.Invoke();
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Ingame, 0);
            CoreUISignals.Instance.onOpenCutscene?.Invoke(1);
            //CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
            //CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Settings);
        }

        public void OnQuit()
        {
            Application.Quit();
        }

        public void OnSettings()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Settings, 2);
        }
    }
}