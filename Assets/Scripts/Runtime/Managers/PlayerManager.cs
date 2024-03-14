
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
        [SerializeField] private Transform playerTrannsform;
        
        
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
            InputSignals.Instance.onPlayerPressedRightMouseButton += OnPlayerPressedRightMouseButton;
            PlayerSignals.Instance.onGetPlayerSpeed += OnGetPlayerSpeed;
            PlayerSignals.Instance.onSetPlayerToCutScenePosition += OnSetPlayerToCutScenePosition;
            


        }

        private void OnSetPlayerToCutScenePosition(int playableEnum)
        {
            switch (playableEnum)
            {
                case (int)PlayableEnum.StandUp:
                    PlayableSignals.Instance.onSetUpCutScene?.Invoke(playableEnum);
                    break;
                case (int)PlayableEnum.Factory:
                {
                    Debug.LogWarning("Factory Started");
                    var position = _cutScenePositionHolderData.cutSceneHolders[playableEnum].cutScenePosition;
                    var rotation = _cutScenePositionHolderData.cutSceneHolders[playableEnum].cutSceneRotation;
                    playerTrannsform.DOMove(position, 1f);
                    playerTrannsform.DORotate(rotation, 1f);
                    PlayableSignals.Instance.onSendInputManagerToReadyForInput?.Invoke(true,(int)PlayableEnum.Factory);
                    break;
                }
                case (int)PlayableEnum.House:
                {
                    var position = _cutScenePositionHolderData.cutSceneHolders[playableEnum].cutScenePosition;
                    var rotation = _cutScenePositionHolderData.cutSceneHolders[playableEnum].cutSceneRotation;
                    playerTrannsform.DOMove(position, 1f);
                    playerTrannsform.DORotate(rotation, 1f);
                    CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
                    break;
                }
                default:
                {
                    var position = _cutScenePositionHolderData.cutSceneHolders[playableEnum].cutScenePosition;
                    var rotation = _cutScenePositionHolderData.cutSceneHolders[playableEnum].cutSceneRotation;
                    playerTrannsform.DOMove(position, 1f);
                    playerTrannsform.DORotate(rotation, 1f).OnComplete(() =>
                    {
                        PlayableSignals.Instance.onSetUpCutScene?.Invoke(playableEnum);
                    });
                    break;
                }
            }
        }

       


        private void OnPlayerPressedRightMouseButton(bool arg0)
        {
            playerAnimationController.OnPlayerPressedRightMouseButton(arg0);
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
            InputSignals.Instance.onIsPlayerReadyToMove -= playerMovementController.OnPlayerReadyToMove;
            InputSignals.Instance.onSendInputParams -= OnGetInputParams;
            InputSignals.Instance.onPlayerPressedLeftControlButton -= OnPlayerPressedLeftControlButton;
            InputSignals.Instance.onPlayerPressedLeftShiftButton -= OnPlayerPressedLeftShiftButton;
            InputSignals.Instance.onPlayerPressedSpaceButton -= OnPlayerPressedSpaceButton;
            InputSignals.Instance.onPlayerPressedRightMouseButton -= OnPlayerPressedRightMouseButton;
            PlayerSignals.Instance.onGetPlayerSpeed -= OnGetPlayerSpeed;
            PlayerSignals.Instance.onSetPlayerToCutScenePosition -= OnSetPlayerToCutScenePosition;
           
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        
       
       
    }
}