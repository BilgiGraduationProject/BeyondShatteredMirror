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
        
        

        internal void OnPlayerPressedLeftControlButton(bool condition)
        {
            _playerAnimator.SetBool("Crouch", condition);
        }
        
        internal void OnPlayerPressedSpaceButton()
        {
            _playerAnimator.SetTrigger("Roll");
        }


        #region Animation Event

        public void AnimEventCancelPlayerMovement()
        {
            Debug.LogWarning("Cancel Anim");
            InputSignals.Instance.onIsInputReadyToUse?.Invoke(false);
        }

        public void AnimEventActivatePlayerMovement()
        {
            InputSignals.Instance.onIsInputReadyToUse?.Invoke(true);
        }

        #endregion


        public void OnPlayerPressedRightMouseButton(bool condition)
        {
            _playerAnimator.SetBool("Fight", condition);
            
        }
    }
    
}