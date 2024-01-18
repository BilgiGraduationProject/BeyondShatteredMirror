using System;
using Runtime.Controllers.Player;
using Runtime.Enums.GameManager;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class PlayerFightingManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerPunchController playerPunchController;
        [SerializeField] private PlayerGunController playerGunController;
        [SerializeField] private PlayerEnemyDetectionController playerEnemyDetectionController;

        #endregion


        #region Private Variables

        

        #endregion

        #endregion


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onPlayerPressedAttackButton += OnPlayerPressedAttackButton;
            InputSignals.Instance.onPlayerPressedMouseButtonRight += OnPlayerPressedMouseRightButton;
            InputSignals.Instance.onPlayerReleasedMouseButtonRight += OnPlayerReleasedMouseButtonRight;
            EnemySignals.Instance.onGetEnemyHealth += OnGetEnemyHealth;
            
            PlayerSignals.Instance.onIsPlayerReadyToPunch += playerPunchController.OnIsPlayerReadyToAttack;
            PlayerSignals.Instance.onIsPlayerReadyToShoot += playerGunController.OnIsPlayerReadyToShoot;
            PlayerSignals.Instance.onPlayerCanShoot += playerGunController.OnPlayerCanShoot;
        }

        private void OnPlayerReleasedMouseButtonRight()
        {
            switch (CoreGameSignals.Instance.onCheckFightStatu?.Invoke())
            {
                case GameFightStateEnum.Idle:
                    break;
                case GameFightStateEnum.Pistol:
                    playerGunController.OnPlayerReleasedMouseButtonRight();
                    PlayerSignals.Instance.onPlayerCanShoot?.Invoke(false);
                    break;
                case GameFightStateEnum.Punch:
                    playerPunchController.Dodge();
                    break;
                
            }
        }

        private void OnPlayerPressedMouseRightButton()
        {
            switch (CoreGameSignals.Instance.onCheckFightStatu?.Invoke())
            {
                case GameFightStateEnum.Idle:
                    break;
                case GameFightStateEnum.Pistol:
                    playerGunController.OnPlayerPressedAimButton();
                    PlayerSignals.Instance.onPlayerCanShoot?.Invoke(true);
                    break;
                case GameFightStateEnum.Punch:
                    playerPunchController.Dodge();
                    
                    break;
                
            }
        }

        private void OnPlayerPressedAttackButton()
        {
            switch (CoreGameSignals.Instance.onCheckFightStatu?.Invoke())
            {
                case GameFightStateEnum.Idle:
                    break;
                case GameFightStateEnum.Pistol:
                    playerGunController.OnPlayerPressedShootButton();
                    break;
                case GameFightStateEnum.Punch:
                    Debug.LogWarning("Punching");
                    playerPunchController.AttackCheck(playerEnemyDetectionController.GetEnemyTransform());
                    break;
                
            }
        }

        private void OnGetEnemyHealth(float enemyHealth)
        {
            playerPunchController.OnGetEnemyHealth(enemyHealth);
        }

        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onPlayerPressedAttackButton -= OnPlayerPressedAttackButton;
            InputSignals.Instance.onPlayerPressedMouseButtonRight -= OnPlayerPressedMouseRightButton;
            InputSignals.Instance.onPlayerReleasedMouseButtonRight -= OnPlayerReleasedMouseButtonRight;
            EnemySignals.Instance.onGetEnemyHealth -= OnGetEnemyHealth;
            
            PlayerSignals.Instance.onIsPlayerReadyToPunch -= playerPunchController.OnIsPlayerReadyToAttack;
            PlayerSignals.Instance.onIsPlayerReadyToShoot -= playerGunController.OnIsPlayerReadyToShoot;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}