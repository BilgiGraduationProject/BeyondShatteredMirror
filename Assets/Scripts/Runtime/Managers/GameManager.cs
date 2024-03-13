using System;
using Runtime.Enums.GameManager;
using Runtime.Enums.Player;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

       [SerializeField] private GameStateEnum gameState;

        #endregion

        #endregion
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            
            CoreGameSignals.Instance.onGameStatusChanged += OnGameStatusChanged;
        }
       

        private void OnGameStatusChanged(GameStateEnum type)
        {
            gameState = type;
            switch (gameState)
            {
                case GameStateEnum.Game:
                    InputSignals.Instance.onIsInputReadyToUse?.Invoke(true);
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
                    InputSignals.Instance.onIsInputReadyToUse?.Invoke(true);
                  
                    break;
                case GameStateEnum.Cutscene:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    InputSignals.Instance.onIsInputReadyToUse?.Invoke(false);
                 
                    break;
                case GameStateEnum.Quit:
                    
                    break;
                case GameStateEnum.Settings:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    InputSignals.Instance.onIsInputReadyToUse?.Invoke(false);
                    break;
                
                case GameStateEnum.Camera:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
                    InputSignals.Instance.onIsInputReadyToUse?.Invoke(false);
                    break;
              
            }
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onGameStatusChanged += OnGameStatusChanged;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}