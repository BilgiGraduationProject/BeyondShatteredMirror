using System;
using Runtime.Enums.Camera;
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
                    InputSignals.Instance.onIsMovementInputReadyToUse?.Invoke(true);
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
                    CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.Play);
                  
                    break;
                case GameStateEnum.Cutscene:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    InputSignals.Instance.onIsMovementInputReadyToUse?.Invoke(false);
                    CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.CutScene);
                 
                    break;
                case GameStateEnum.Quit:
                    
                    break;
                case GameStateEnum.Settings:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    InputSignals.Instance.onIsMovementInputReadyToUse?.Invoke(false);
                    break;
                
                case GameStateEnum.UI:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
                    InputSignals.Instance.onIsMovementInputReadyToUse?.Invoke(false);
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