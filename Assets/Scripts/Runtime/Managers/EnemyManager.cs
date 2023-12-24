using System;
using System.Collections.Generic;
using Runtime.Controllers.Enemy;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Enums.Enemy;
using Runtime.Enums.Player;
using Runtime.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private EnemyAnimationController enemyAnimationController;
        [SerializeField] private EnemyController enemyController;
        [SerializeField] private EnemyType enemyType;
        [SerializeField] private Rigidbody enemyRigidbody;

        #endregion

        #region Private Variables

        private EnemyData _enemyData;

        #endregion

        #endregion


        private void Awake()
        {
            _enemyData = GetEnemyData();
            SendEnemyDataToControllers();
        }

        private void SendEnemyDataToControllers() => enemyController.GetEnemyData(_enemyData);
       

        private EnemyData GetEnemyData() => Resources.Load<CD_Enemy>("Data/CD_Enemy").Data[(int)enemyType];
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EnemySignals.Instance.onChangeEnemyAnimationState += enemyAnimationController.ChangeEnemyAnimationState;
            EnemySignals.Instance.onCheckEnemyHealth += OnCheckEnemyHealth;
            EnemySignals.Instance.onAddEnemyToForce += OnAddForceToEnemy;
        

        }
        

        private void OnAddForceToEnemy(float force)
        {
            enemyRigidbody.AddForce(transform.forward * -force, ForceMode.Impulse);
        }

        private void OnCheckEnemyHealth(GameObject enemyObj)
        {
            if (enemyObj.GetInstanceID() != gameObject.GetInstanceID()) return;
            EnemySignals.Instance.onGetEnemyHealth?.Invoke(enemyController.GetEnemyHealth());
        }


        private void UnSubscribeEvents()
        {
            EnemySignals.Instance.onChangeEnemyAnimationState -= enemyAnimationController.ChangeEnemyAnimationState;
            EnemySignals.Instance.onCheckEnemyHealth -= OnCheckEnemyHealth;
            EnemySignals.Instance.onAddEnemyToForce -= OnAddForceToEnemy;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}