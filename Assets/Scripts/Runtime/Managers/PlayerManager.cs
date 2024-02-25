
using System;
using System.Collections.Generic;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.GameManager;
using Runtime.Enums.Player;
using Runtime.Keys.Input;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerMovementController playerMovementController;
        [SerializeField] private PlayerAnimationController playerAnimationController;
        
        #endregion

        #region Private Variables
        private PlayerData _playerData;
        private readonly string _playerDataPath = "Data/CD_Player";
        private Camera _camera;
        #endregion

        #endregion

        private void Awake()
        {
            if (Camera.main != null) _camera = Camera.main;
            _playerData = GetPlayerData();
            SendPlayerDataToControllers();
            SendCameraTransformToMovementController(_camera);
        }

        private void SendCameraTransformToMovementController(Camera cameraTransform)
        {
            playerMovementController.GetCameraTransform(cameraTransform);
        }


        private void SendPlayerDataToControllers()
        {
            playerMovementController.GetPlayerData(_playerData);
        }

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>(_playerDataPath).Data;
        private void OnGetInputParams(InputParams inputParams) => playerMovementController.GetInputParams(inputParams);
        private void OnGetPlayerSpeed(float speed) => playerAnimationController.GetPlayerSpeed(speed);
        
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onIsPlayerReadyToMove += playerMovementController.OnPlayerReadyToMove;
            InputSignals.Instance.onSendInputParams += OnGetInputParams;
            InputSignals.Instance.onPlayerPressedLeftControlButton += OnPlayerPressedLeftControlButton;
            InputSignals.Instance.onPlayerPressedLeftShiftButton += OnPlayerPressedLeftShiftButton;
            InputSignals.Instance.onPlayerPressedSpaceButton += OnPlayerPressedSpaceButton;
            PlayerSignals.Instance.onGetPlayerSpeed += OnGetPlayerSpeed;


        }

        private void OnPlayerPressedSpaceButton()
        {
            
            playerMovementController.OnPlayerPressedSpaceButton();
        }


        private void OnPlayerPressedLeftShiftButton(bool condition)
        {
            playerMovementController.OnPlayerPressedLeftShiftButton(condition);
        }

        private void OnPlayerPressedLeftControlButton(bool condition)
        {
            playerAnimationController.OnPlayerPressedLeftControlButton(condition);
        }


        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onSendInputParams -= OnGetInputParams;
            InputSignals.Instance.onPlayerPressedLeftControlButton -= OnPlayerPressedLeftControlButton;
            InputSignals.Instance.onPlayerPressedLeftShiftButton -= OnPlayerPressedLeftShiftButton;
            InputSignals.Instance.onPlayerPressedSpaceButton -= playerAnimationController.OnPlayerPressedSpaceButton;
            PlayerSignals.Instance.onGetPlayerSpeed -= OnGetPlayerSpeed;
           
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        
       
       
    }
}