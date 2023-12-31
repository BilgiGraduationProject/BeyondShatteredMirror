using System;
using System.Collections.Generic;
using Runtime.Controllers.Enemy;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.Enemy;
using Runtime.Enums.Player;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private EnemyAnimationController enemyAnimationController;
         [SerializeField] private EnemyHealthController enemyHealthController;
         [SerializeField] private EnemyMeshController enemyMeshController;
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private Rigidbody enemyRigidbody;

        #endregion

        #region Private Variables

        private EnemyData _enemyData;
        private bool _isEnemyDead;

        #endregion

        #endregion


        private void Awake()
        {
            _enemyData = GetEnemyData();
            SendEnemyDataToControllers();
        }

        private void SendEnemyDataToControllers() => enemyHealthController.GetEnemyData(_enemyData);
       

        private EnemyData GetEnemyData() => Resources.Load<CD_Enemy>("Data/CD_Enemy").Data[(int)enemyType];
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EnemySignals.Instance.onChangeEnemyAnimationState += enemyAnimationController.ChangeEnemyAnimationState;
            EnemySignals.Instance.onCheckEnemyHealth += OnCheckEnemyHealth;
            EnemySignals.Instance.onPlayerBodyCollidedWithEnemey += OnDamageEnemy;
            EnemySignals.Instance.onShowEnemyHealthBar += OnShowEnemyHealthBar;
            EnemySignals.Instance.onHideEnemyHealthBar += OnHideEnemyHealthBar;
            
        }

        private void OnHideEnemyHealthBar(GameObject enemyObj)
        {
            if(gameObject.GetInstanceID() != enemyObj.GetInstanceID()) return;
            enemyHealthController.HideEnemyHealthBar();
        }

        private void OnShowEnemyHealthBar(GameObject enemyObj)
        {
            if(gameObject.GetInstanceID() != enemyObj.GetInstanceID()) return;
            enemyHealthController.ShowEnemyHealthBar();
        }

        private void OnDamageEnemy(EnemyAnimationState hitType, GameObject enemyObj)
        {
            if(gameObject.GetInstanceID() != enemyObj.GetInstanceID()) return;
            if (_isEnemyDead) return;
            enemyAnimationController.ChangeEnemyAnimationState(hitType);
            enemyHealthController.AddDamageToEnemy(hitType);
            AddForceToEnemy(hitType);
            if (enemyHealthController.GetEnemyHealth() <= 0)
            {
                enemyAnimationController.ChangeEnemyAnimationState(EnemyAnimationState.Dead);
                EnemySignals.Instance.onEnemyDied?.Invoke(transform);
                _isEnemyDead = true;
            }
        }

        private void AddForceToEnemy(EnemyAnimationState hitType)
        {
            switch (hitType)
            {
                case EnemyAnimationState.FaceHit:
                    enemyRigidbody.AddForce(transform.forward * -_enemyData.FaceForce, ForceMode.VelocityChange);
                    break;
                case EnemyAnimationState.KickBodyHit:
                    enemyRigidbody.AddForce(transform.forward * -_enemyData.KickForce, ForceMode.VelocityChange);
                    break;
                case EnemyAnimationState.PunchBodyHit:
                    enemyRigidbody.AddForce(transform.forward * -_enemyData.FaceForce, ForceMode.VelocityChange);
                    break;
                    
            }
        }


        private void OnCheckEnemyHealth(GameObject enemyObj)
        {
            if (enemyObj.GetInstanceID() != gameObject.GetInstanceID()) return;
            EnemySignals.Instance.onGetEnemyHealth?.Invoke(enemyHealthController.GetEnemyHealth());
        }


        private void UnSubscribeEvents()
        {
            EnemySignals.Instance.onChangeEnemyAnimationState -= enemyAnimationController.ChangeEnemyAnimationState;
            EnemySignals.Instance.onCheckEnemyHealth -= OnCheckEnemyHealth;
            EnemySignals.Instance.onPlayerBodyCollidedWithEnemey -= OnDamageEnemy;
            EnemySignals.Instance.onShowEnemyHealthBar -= OnShowEnemyHealthBar;
            EnemySignals.Instance.onHideEnemyHealthBar -= OnHideEnemyHealthBar;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}