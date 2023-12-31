using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Controllers.Enemy;
using Runtime.Enums.GameManager;
using Runtime.Enums.Player;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Controllers.Player
{
    public class PlayerFightController : MonoBehaviour
    {
        #region Self Variables
        

        #region Private Variables
        private bool _isAttackingEnemy;
        

        private float _enemyHealth;
        private bool _isPlayerReadyToAttack;

        #endregion

        #endregion

        internal void OnGetEnemyHealth(float enemyHealth) => _enemyHealth = enemyHealth;
        internal void OnIsPlayerReadyToAttack(bool condition) => _isPlayerReadyToAttack = condition;
        
        public void AttackCheck(GameObject enemyTransform)
        {
            if (!_isPlayerReadyToAttack) return;

            if (enemyTransform == null)
            {
                Attack(null, 0);
                return;
            }
            EnemySignals.Instance.onCheckEnemyHealth?.Invoke(enemyTransform);
            
            if (_enemyHealth <= 0)
            {
                Attack(null,0);
               
            }
            else
            {
                Attack(enemyTransform, TargetDistance(enemyTransform.transform));
            }
            

        }

        private float TargetDistance(Transform enemyTransform)
        {
            return Vector3.Distance(transform.position,enemyTransform.position);
        }
        private void Attack(GameObject enemyTransform, float targetDistance)
        {
            if (enemyTransform == null)
            {
                var randomRange = Random.Range(0, Enum.GetValues(typeof(PlayerCloseAttackAnimationState)).Length);
                var playerCloseAttack = Enum.GetNames(typeof(PlayerCloseAttackAnimationState));
                AttackType(playerCloseAttack[randomRange],  null, 0);
            }

            else if(targetDistance > 3)
            {
                var randomRange = Random.Range(0, Enum.GetValues(typeof(PlayerFarAttackAnimationState)).Length);
                var playerFarAttack = Enum.GetNames(typeof(PlayerFarAttackAnimationState));
                AttackType(playerFarAttack[randomRange],enemyTransform,1f);
            }
            
            else if (targetDistance < 3)
            {
                var randomRange = Random.Range(0, Enum.GetValues(typeof(PlayerCloseAttackAnimationState)).Length);
                var playerCloseAttack = Enum.GetNames(typeof(PlayerCloseAttackAnimationState));
                AttackType(playerCloseAttack[randomRange], enemyTransform, 1f);
            }
            
        }

        private void AttackType(string playerAnimationType, GameObject enemyTarget, float movementDuration)
        {
            PlayerSignals.Instance.onTriggerAttackAnimationState?.Invoke(playerAnimationType);
            
            MoveTowardsToTarget(enemyTarget, movementDuration);
            
          
          
         }

        private void MoveTowardsToTarget(GameObject enemyTarget, float movementDuration)
        {
            if(enemyTarget == null) return;
            transform.DOLookAt(enemyTarget.transform.position, .2f);
            transform.DOMove(TargetOffSet(enemyTarget.transform.position), movementDuration);
        }

        private Vector3 TargetOffSet(Vector3 enemyTransform)
        {
            return Vector3.MoveTowards(enemyTransform,transform.position,.95f);
        }


       

      
    }
}