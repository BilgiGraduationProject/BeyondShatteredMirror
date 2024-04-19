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
        }

        private EnemyData GetEnemyData() => Resources.Load<CD_Enemy>("Data/CD_Enemy").Data[(int)enemyType];
        

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
           
          
            
        }
        
        private void UnSubscribeEvents()
        {
            
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}