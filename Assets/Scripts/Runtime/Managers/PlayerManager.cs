
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
using StarterAssets;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private ThirdPersonController playerThirdPersonController;
        [SerializeField] private PlayerAnimationController playerAnimationController;
        [SerializeField] private PlayerHitDetectionController playerHitDetectionController;
        
        [SerializeField] private Transform playerTransform;
        
        
        #endregion

        #region Private Variables
        private Camera _camera;
        private CD_CutScenePositionHolder _cutScenePositionHolderData;
        #endregion

        #endregion

        private void Awake()
        {
            if (Camera.main != null) _camera = Camera.main;
           _cutScenePositionHolderData = GetCutScenePositionHolderData();
            SendCameraTransformToMovementController(_camera);
        }

        private static CD_CutScenePositionHolder GetCutScenePositionHolderData() => Resources
            .Load<CD_CutScenePositionHolder>("Data/CD_CutScenePositionHolder");
       

        private void SendCameraTransformToMovementController(Camera cameraTransform)
        {
            playerHitDetectionController.GetCameraTransform(cameraTransform);
        }

        
        
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
         
            InputSignals.Instance.onIsPlayerReadyToMove += playerThirdPersonController.OnIsPlayerReadyToMove;
            InputSignals.Instance.onPlayerPressedLeftControlButton += OnPlayerPressedLeftControlButton;
            InputSignals.Instance.onPlayerPressedSpaceButton += OnPlayerPressedSpaceButton;
            InputSignals.Instance.onPlayerPressedPickUpButton += playerHitDetectionController.OnPlayerPressedPickUpButton;
            InputSignals.Instance.onPlayerPressedDropItemButton +=
                playerHitDetectionController.OnPlayerPressedDropItemButton;
            PlayerSignals.Instance.onSetPlayerToCutScenePosition += OnSetPlayerToCutScenePosition;
            PlayerSignals.Instance.onSetAnimationBool += playerAnimationController.OnSetBoolAnimation;
            PlayerSignals.Instance.onSetAnimationTrigger += playerAnimationController.OnSetTriggerAnimation;
            PlayerSignals.Instance.onSetCombatCount += playerAnimationController.OnSetCombatCount;
            PlayerSignals.Instance.onPlayerIsRolling += playerThirdPersonController.OnPlayerIsRolling;
            PlayerSignals.Instance.onPlayerReadyToKillTheEnemy += playerHitDetectionController.OnPlayerReadyToKillTheEnemy;
            PlayerSignals.Instance.onSetAnimationPlayerSpeed += playerAnimationController.OnSetAnimationPlayerSpeed;





        }

        private void OnSetPlayerParentToLevel(Transform levelTransform)
        {
        }

        private void OnSetPlayerToCutScenePosition(PlayableEnum playableEnum)
        {
            Debug.LogWarning("SetUppedPosition");
            var position = PlayerSignals.Instance.onGetLevelCutScenePosition(playableEnum);
            var rotation = _cutScenePositionHolderData.cutSceneHolders[(int)playableEnum].cutSceneRotation;
            transform.position = position.position;
            playerTransform.DORotate(rotation, 1f);

        }

        

        

        private void OnPlayerPressedSpaceButton()
        {
            playerThirdPersonController.OnPlayerPressedSpaceButton();
        }

        

        private void OnPlayerPressedLeftControlButton(bool condition)
        {
            PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Crouch, condition);
        }
        


        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onIsPlayerReadyToMove -= playerThirdPersonController.OnIsPlayerReadyToMove;
            InputSignals.Instance.onPlayerPressedLeftControlButton -= OnPlayerPressedLeftControlButton;
            InputSignals.Instance.onPlayerPressedSpaceButton -= OnPlayerPressedSpaceButton;
            InputSignals.Instance.onPlayerPressedPickUpButton -= playerHitDetectionController.OnPlayerPressedPickUpButton;
            InputSignals.Instance.onPlayerPressedDropItemButton -=
                playerHitDetectionController.OnPlayerPressedDropItemButton;
            PlayerSignals.Instance.onSetPlayerToCutScenePosition -= OnSetPlayerToCutScenePosition;
            PlayerSignals.Instance.onSetAnimationBool -= playerAnimationController.OnSetBoolAnimation;
            PlayerSignals.Instance.onSetAnimationTrigger -= playerAnimationController.OnSetTriggerAnimation;
            PlayerSignals.Instance.onSetCombatCount -= playerAnimationController.OnSetCombatCount;
            PlayerSignals.Instance.onPlayerIsRolling -= playerThirdPersonController.OnPlayerIsRolling;
            PlayerSignals.Instance.onPlayerReadyToKillTheEnemy -= playerHitDetectionController.OnPlayerReadyToKillTheEnemy;
            PlayerSignals.Instance.onSetAnimationPlayerSpeed -= playerAnimationController.OnSetAnimationPlayerSpeed;
           
            
        }


       

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        
       
       
    }
}