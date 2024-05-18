using System;
using Runtime.Data.UnityObject;
using Runtime.Enums.Camera;
using Runtime.Enums.GameManager;
using Runtime.Enums.Playable;
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
       private PlayableEnum currentGameState;

        #endregion
        

        #endregion

        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onGameStatusChanged += OnGameStatusChanged;
            CoreGameSignals.Instance.onGetGameState += () => gameState;
            CoreGameSignals.Instance.onGameManagerGetCurrentGameState += OnGameManagerGetCurrentGameState;
            CoreGameSignals.Instance.onSendCurrentGameStateToUIText += SendCurrentGameState;
        }

        private PlayableEnum SendCurrentGameState()
        {
            return currentGameState;
        }

        private void OnGameManagerGetCurrentGameState(PlayableEnum condition)
        {
            currentGameState = condition;
        }


        private void OnGameStatusChanged(GameStateEnum type)
        {
            gameState = type;
            Debug.LogWarning("Game State Changed To this" + type);
            switch (gameState)
            {
                case GameStateEnum.Game:
                    InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(true);
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
                    CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.Play);
                    break;
                case GameStateEnum.Cutscene:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
                    CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.CutScene);
                 
                    break;
                case GameStateEnum.Quit:
                    
                    break;
                case GameStateEnum.UI:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
                    CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.Play);
                    break;
                
                case GameStateEnum.Start:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
                    break;
                case GameStateEnum.Capture:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(true);
                    break;
            }
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onGameStatusChanged -= OnGameStatusChanged;
            CoreGameSignals.Instance.onGetGameState -= () => gameState;
            CoreGameSignals.Instance.onGameManagerGetCurrentGameState -= OnGameManagerGetCurrentGameState;
            CoreGameSignals.Instance.onSendCurrentGameStateToUIText -= SendCurrentGameState;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}