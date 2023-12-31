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

        [SerializeField] private GameObject enemyHealthHolder;
        [SerializeField] private Transform enemyHealthBar;

        #endregion

        #region Private Variables

        private EnemyData _enemyData;
        private float _enemyHealth;
        private GameObject _enemyObj;
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

            if (_enemyHealth > 0)
            {
                enemyHealthBar.localScale = new Vector3(_enemyHealth / 100, 1f);
            }
            else
            {
                enemyHealthBar.localScale = new Vector3(0, 1f, 1f);
                enemyHealthHolder.SetActive(false);
            }
            




        }

        internal void ShowEnemyHealthBar()
        {
            enemyHealthHolder.SetActive(true);
        }

        internal void HideEnemyHealthBar()
        {
            enemyHealthHolder.SetActive(false);
        }
    }
}