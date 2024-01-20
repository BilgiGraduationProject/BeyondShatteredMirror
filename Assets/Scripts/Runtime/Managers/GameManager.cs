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
       [SerializeField] private GameFightStateEnum gameFightStateEnum;

        #endregion

        #endregion

        private void Start()
        {
            // Don't need this. I already use on StartPanel - Play button
            OnGameStatusChanged(gameState);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            
            CoreGameSignals.Instance.onGameStatusChanged += OnGameStatusChanged;
            CoreGameSignals.Instance.onCheckFightStatu += OnCheckFightStatu;
            CoreGameSignals.Instance.onChangeGameFightState += OnChangeGameFightState;
        }

        private void OnChangeGameFightState(GameFightStateEnum type)
        {
            gameFightStateEnum = type;
            switch (type)
            {
                case GameFightStateEnum.Idle:
                    PlayerSignals.Instance.onIsPlayerReadyToPunch?.Invoke(false);
                    PlayerSignals.Instance.onIsPlayerReadyToShoot?.Invoke(false);
                    break;
                case GameFightStateEnum.Punch:
                    PlayerSignals.Instance.onIsPlayerReadyToPunch.Invoke(true);
                    PlayerSignals.Instance.onIsPlayerReadyToShoot?.Invoke(false);
                    break;
                case GameFightStateEnum.Pistol:
                    PlayerSignals.Instance.onIsPlayerReadyToShoot?.Invoke(true);
                    PlayerSignals.Instance.onIsPlayerReadyToPunch?.Invoke(false);
                    break;
                
            }
        }


        private GameFightStateEnum OnCheckFightStatu() => gameFightStateEnum;
       

        private void OnGameStatusChanged(GameStateEnum type)
        {
            gameState = type;
            switch (gameState)
            {
                case GameStateEnum.GameStart:
                    CoreGameSignals.Instance.onIsInputReady?.Invoke(true);
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
                    break;
                case GameStateEnum.Cutscene:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    break;
                case GameStateEnum.CancelPlayerMovement:
                    CoreGameSignals.Instance.onIsInputReady?.Invoke(false);
                    break;
                case GameStateEnum.ActivatePlayerMovement:
                    CoreGameSignals.Instance.onIsInputReady?.Invoke(true);
                    break;
                case GameStateEnum.Quit:
                    
                    break;
              
            }
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onGameStatusChanged += OnGameStatusChanged;
            CoreGameSignals.Instance.onCheckFightStatu += OnCheckFightStatu;
            CoreGameSignals.Instance.onChangeGameFightState += OnChangeGameFightState;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}