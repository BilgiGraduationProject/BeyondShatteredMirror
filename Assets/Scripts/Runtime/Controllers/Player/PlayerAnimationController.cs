using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Enums.GameManager;
using Runtime.Enums.Player;
using Runtime.Keys.Input;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Animator _playerAnimator;

        

        #endregion

        #region Private Variables

        private readonly string _speed = "Speed";
        private readonly string _crouch = "Crouch";
        private readonly string _roll = "Roll";

        #endregion

        #endregion

        
        
        
        
        internal void OnSetBoolAnimation(PlayerAnimationState playerEnum, bool condition)
        {
            _playerAnimator.SetBool(playerEnum.ToString(), condition);
        }

        internal void OnSetTriggerAnimation(PlayerAnimationState playerEnum)
        {
            _playerAnimator.SetTrigger(playerEnum.ToString());
        }
        
        internal void OnSetCombatCount(float combatCount)
        {
            _playerAnimator.SetFloat(PlayerAnimationState.PunchCount.ToString(),combatCount);
        }

        #region Animation Event

        public void AnimEventCancelPlayerMovement()
        {
            InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
        }

        public void AnimEventActivatePlayerMovement()
        {
            InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(true);
        }


        public void AnimEventCancelPlayerCombat()
        {
            InputSignals.Instance.onIsReadyForCombat?.Invoke(false);
            InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
        }

        public void AnimEventActivatePlayerCombat()
        {
            InputSignals.Instance.onIsReadyForCombat?.Invoke(true);
            InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(true);
            PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Attack, false);
        }

        public void AnimEventOnPlayerRolling()
        {
            InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
            PlayerSignals.Instance.onPlayerIsRolling?.Invoke(true);
        }

        public void AnimEventOnPlayIsNotRolling()
        {
            InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(true);
            PlayerSignals.Instance.onPlayerIsRolling?.Invoke(false);
            PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Roll,false);
           
        }


        public void AnimEventOnKillEnemy()
        {
            PlayerSignals.Instance.onPlayerReadyToKillTheEnemy?.Invoke();
        }


        #endregion


        public void OnSetAnimationPlayerSpeed(float speed)
        {
            _playerAnimator.SetFloat("Speed",speed);
        }
        
        public void StartDealDamage()
        {
            foreach (var controller in GetComponentsInChildren<PlayerDamageController>())
            {
                AnimatorStateInfo stateInfo = _playerAnimator.GetCurrentAnimatorStateInfo(0);
                print(stateInfo);
                controller.StartDealDamage();
            }
        }
        
        public void EndDealDamage()
        {
            foreach (var controller in GetComponentsInChildren<PlayerDamageController>())
            {
                controller.EndDealDamage();
            }
        }
    }
}