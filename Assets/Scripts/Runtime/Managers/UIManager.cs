using System;
using Runtime.Enums.GameManager;
using Runtime.Signals;
using UnityEngine;
using Runtime.Enums.UI;

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
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
            // TODO: Add more shows to beatify the game start.
            CoreUISignals.Instance.onCloseAllPanels?.Invoke();
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Ingame, 0);
        }

        public void OnQuit()
        {
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Quit);
            Application.Quit();
        }

        public void OnSettings()
        {
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Settings);
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Settings, 2);
        }
    }
}