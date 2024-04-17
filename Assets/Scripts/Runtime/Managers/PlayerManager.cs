
using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.GameManager;
using Runtime.Enums.Playable;
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
        [SerializeField] private PlayerHitDetectionController playerHitDetectionController;
        
        [SerializeField] private Transform playerTransform;
        
        
        #endregion

        #region Private Variables
        private PlayerData _playerData;
        private readonly string _playerDataPath = "Data/CD_Player";
        private Camera _camera;
        private CD_CutScenePositionHolder _cutScenePositionHolderData;
        #endregion

        #endregion

        private void Awake()
        {
            if (Camera.main != null) _camera = Camera.main;
            _playerData = GetPlayerData();
           _cutScenePositionHolderData = GetCutScenePositionHolderData();
            SendPlayerDataToControllers();
            SendCameraTransformToMovementController(_camera);
        }

        private static CD_CutScenePositionHolder GetCutScenePositionHolderData() => Resources
            .Load<CD_CutScenePositionHolder>("Data/CD_CutScenePositionHolder");
       

        private void SendCameraTransformToMovementController(Camera cameraTransform)
        {
            playerMovementController.GetCameraTransform(cameraTransform);
            playerHitDetectionController.GetCameraTransform(cameraTransform);
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
            PlayerSignals.Instance.onSetPlayerToCutScenePosition += OnSetPlayerToCutScenePosition;
            PlayerSignals.Instance.onSetAnimationBool += playerAnimationController.OnSetBoolAnimation;
            PlayerSignals.Instance.onSetAnimationTrigger += playerAnimationController.OnSetTriggerAnimation;
            PlayerSignals.Instance.onSetCombatCount += playerAnimationController.OnSetCombatCount;
            PlayerSignals.Instance.onPlayerCollidedWithObstacle += playerMovementController.OnPlayerCollidedWithObstacle;
            PlayerSignals.Instance.onIsPlayerFalling += playerMovementController.OnIsPlayerFalling;
            PlayerSignals.Instance.onIsKillRoll += playerMovementController.OnIsKillRoll;
            PlayerSignals.Instance.onPlayerInterectWithObject +=
                playerHitDetectionController.OnPlayerInteractWithObject;
            PlayerSignals.Instance.onGetPlayerTransform += OnGetPlayerManagerTransform;
            PlayerSignals.Instance.onCanPlayerInteractWithSomething += playerHitDetectionController.OnCanPlayerInteractWithSomething;
            PlayerSignals.Instance.onTakeInteractableObject += playerHitDetectionController.OnGetInteractableObject;



        }

        private void OnSetPlayerToCutScenePosition(PlayableEnum playableEnum)
        {
            var position = _cutScenePositionHolderData.cutSceneHolders[(int)playableEnum].cutScenePosition;
            var rotation = _cutScenePositionHolderData.cutSceneHolders[(int)playableEnum].cutSceneRotation;
           
        }


        private void SetPlayerToCutScenePosition(Transform position, float duration, Vector3 rotation)
        {
            playerTransform.DOMove(position.position, duration);
            playerTransform.DORotate(rotation, duration);
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
            PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Crouch, condition);
        }
        
        private Transform OnGetPlayerManagerTransform()
        {
            return playerTransform;
        }


        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onIsPlayerReadyToMove -= playerMovementController.OnPlayerReadyToMove;
            InputSignals.Instance.onSendInputParams -= OnGetInputParams;
            InputSignals.Instance.onPlayerPressedLeftControlButton -= OnPlayerPressedLeftControlButton;
            InputSignals.Instance.onPlayerPressedLeftShiftButton -= OnPlayerPressedLeftShiftButton;
            InputSignals.Instance.onPlayerPressedSpaceButton -= OnPlayerPressedSpaceButton;
            PlayerSignals.Instance.onGetPlayerSpeed -= OnGetPlayerSpeed;
            PlayerSignals.Instance.onSetPlayerToCutScenePosition -= OnSetPlayerToCutScenePosition;
            PlayerSignals.Instance.onSetAnimationBool -= playerAnimationController.OnSetBoolAnimation;
            PlayerSignals.Instance.onSetAnimationTrigger -= playerAnimationController.OnSetTriggerAnimation;
            PlayerSignals.Instance.onSetCombatCount -= playerAnimationController.OnSetCombatCount;
            PlayerSignals.Instance.onIsPlayerFalling -= playerMovementController.OnIsPlayerFalling;
            PlayerSignals.Instance.onIsKillRoll -= playerMovementController.OnIsKillRoll;
            PlayerSignals.Instance.onPlayerInterectWithObject -=
                playerHitDetectionController.OnPlayerInteractWithObject;
            PlayerSignals.Instance.onGetPlayerTransform -= OnGetPlayerManagerTransform;
            PlayerSignals.Instance.onCanPlayerInteractWithSomething -= playerHitDetectionController.OnCanPlayerInteractWithSomething;
            PlayerSignals.Instance.onTakeInteractableObject -= playerHitDetectionController.OnGetInteractableObject;
            
        }


       

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        
       
       
    }
}