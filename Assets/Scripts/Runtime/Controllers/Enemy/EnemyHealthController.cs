using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Enums.Enemy;
using Runtime.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Controllers.Enemy
{
    public class EnemyHealthController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private CapsuleCollider enemyBoxCollider;

        #endregion

        #region Private Variables

        private EnemyData _enemyData;
        private float _enemyHealth;
        #endregion

        #endregion

        
        internal float GetEnemyHealth() => _enemyHealth;

        internal void GetEnemyData(EnemyData enemyData)
        {
            _enemyData = enemyData;
            _enemyHealth = enemyData.EnemyHealth;
        } 

        public void AddDamageToEnemy(EnemyAnimationState hitType)
        {
            switch (hitType)
            {
                case EnemyAnimationState.FaceHit:
                    _enemyHealth -= _enemyData.FaceHitDamage;
                    break;
                case EnemyAnimationState.KickBodyHit:
                    _enemyHealth -= _enemyData.KickHitDamage;
                    break;
                case EnemyAnimationState.PunchBodyHit:
                    _enemyHealth -= _enemyData.PunchHitDamage;
                    break;
                
            }
            
            
            
            
        }
        
    }
}