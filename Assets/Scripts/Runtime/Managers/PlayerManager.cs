
using System.Collections.Generic;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
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
        [SerializeField] private PlayerEnemyDetectionController playerEnemyDetectionController;
        [SerializeField] private PlayerFightController playerFightController;
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
            playerEnemyDetectionController.GetCameraTransform(cameraTransform);
        }


        private void SendPlayerDataToControllers()
        {
            playerEnemyDetectionController.GetPlayerData(_playerData);
            playerMovementController.GetPlayerData(_playerData);
        }

        private PlayerData GetPlayerData() => Resources.Load<CD_Player>(_playerDataPath).Data;

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onIsPlayerReadyToMove += OnIsPlayerReadyToMove;
            
            InputSignals.Instance.onPlayerPressedMovementButton += OnInputPressedForMovement;
            InputSignals.Instance.onPlayerPressedRunButton += OnPlayerPressedRunButton;
            InputSignals.Instance.onPlayerReleaseRunButton += OnPlayerReleaseRunButton;
            InputSignals.Instance.onPlayerPressedCrouchButton += playerAnimationController.OnPlayerPressCrouchButton;
            InputSignals.Instance.onPlayerPressedRollButton += playerMovementController.OnPlayerPressedRollButton;
            InputSignals.Instance.onPlayerPressedAttackButton += playerEnemyDetectionController.OnPlayerPressedAttackButton;
            InputSignals.Instance.onPlayerPressedCounterButton += playerEnemyDetectionController.OnPlayerPressedCounterButton;
            
            EnemySignals.Instance.onGetEnemyHealth += OnGetEnemyHealth;
            
            PlayerSignals.Instance.onChangePlayerAnimationState += playerAnimationController.OnChangePlayerAnimationState;
            PlayerSignals.Instance.onTriggerAttackAnimationState +=
                playerAnimationController.OnTriggerAttackAnimationState;
            PlayerSignals.Instance.onIsPlayerReadyToAttack += playerFightController.OnIsPlayerReadyToAttack;


        }

        private void OnGetEnemyHealth(float enemyHealth)
        {
            playerFightController.OnGetEnemyHealth(enemyHealth);
        }


        #region Movement

        private void OnIsPlayerReadyToMove(bool condition)
        {
            playerMovementController.OnIsPlayerReadyToMove(condition);
        }
        private void OnInputPressedForMovement(InputParams inputParams)
        {
            playerMovementController.OnUpdateParams(inputParams);
            playerEnemyDetectionController.OnUpdateParams(inputParams);
        }

        #endregion
        

        #region Running
        private void OnPlayerReleaseRunButton()
        {
            playerMovementController.OnPlayerReleaseRunButton();
        }

        private void OnPlayerPressedRunButton()
        {
            playerMovementController.OnPlayerPressedRunButton();
        }
      
        #endregion
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onIsPlayerReadyToMove -= OnIsPlayerReadyToMove;
            
            InputSignals.Instance.onPlayerPressedMovementButton -= OnInputPressedForMovement;
            InputSignals.Instance.onPlayerPressedRunButton -= OnPlayerPressedRunButton;
            InputSignals.Instance.onPlayerReleaseRunButton -= OnPlayerReleaseRunButton;
            InputSignals.Instance.onPlayerPressedCrouchButton -= playerAnimationController.OnPlayerPressCrouchButton;
            InputSignals.Instance.onPlayerPressedRollButton -= playerMovementController.OnPlayerPressedRollButton;
            InputSignals.Instance.onPlayerPressedAttackButton -= playerEnemyDetectionController.OnPlayerPressedAttackButton;
            InputSignals.Instance.onPlayerPressedCounterButton -= playerEnemyDetectionController.OnPlayerPressedCounterButton;

            EnemySignals.Instance.onGetEnemyHealth -= OnGetEnemyHealth;
            
            PlayerSignals.Instance.onChangePlayerAnimationState -= playerAnimationController.OnChangePlayerAnimationState;
            PlayerSignals.Instance.onTriggerAttackAnimationState -=
                playerAnimationController.OnTriggerAttackAnimationState;
            PlayerSignals.Instance.onIsPlayerReadyToAttack -= playerFightController.OnIsPlayerReadyToAttack;
            
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}