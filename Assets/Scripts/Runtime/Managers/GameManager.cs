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
        }
       

        private void OnGameStatusChanged(GameStateEnum type)
        {
            gameState = type;
            switch (gameState)
            {
                case GameStateEnum.GameStart:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(false);
                    break;
                case GameStateEnum.Cutscene:
                    InputSignals.Instance.onChangeMouseVisibility?.Invoke(true);
                    break;
                case GameStateEnum.CancelPlayerMovement:
                    break;
                case GameStateEnum.ActivatePlayerMovement:
                    break;
                case GameStateEnum.Quit:
                    
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