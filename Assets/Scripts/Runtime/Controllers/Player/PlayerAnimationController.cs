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

        

        internal void GetPlayerSpeed(float speed)
        {
            _playerAnimator.SetFloat(_speed,speed);
        }
        
        
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
            InputSignals.Instance.onIsMovementInputReadyToUse?.Invoke(false);
        }

        public void AnimEventActivatePlayerMovement()
        {
            InputSignals.Instance.onIsMovementInputReadyToUse?.Invoke(true);
        }


        public void AnimEventCancelPlayerCombat()
        {
            InputSignals.Instance.onIsReadyForCombat?.Invoke(false);
            InputSignals.Instance.onIsMovementInputReadyToUse?.Invoke(false);
        }

        public void AnimEventActivatePlayerCombat()
        {
            InputSignals.Instance.onIsReadyForCombat?.Invoke(true);
            InputSignals.Instance.onIsMovementInputReadyToUse?.Invoke(true);
            PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Attack, false);
        }


        #endregion
      


        
    }
    
}