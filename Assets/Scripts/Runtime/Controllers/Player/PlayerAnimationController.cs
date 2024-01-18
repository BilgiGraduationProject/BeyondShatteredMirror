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

        [SerializeField] private Animator playerAnimator;
        private bool _isCrouching;
        private float _playerSpeed;
        private readonly string _speed = "Speed";

        [Header("Animation Layers")] 
        [SerializeField] private Transform holster;
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform pistol;

        #endregion

        #endregion

        private void Awake()
        {
            playerAnimator.SetLayerWeight(0,0);
            
        }

        internal void OnChangePlayerAnimationState(PlayerAnimationState animationState, bool condition) =>  
            playerAnimator.SetBool(animationState.ToString(), condition);
        internal void OnTriggerAttackAnimationState(string attackState) => playerAnimator.SetTrigger(attackState);
        
        public void OnChangeAnimationLayerWeight(int layerIndex, float weightIndex,float duration)
        {
            StartCoroutine(ChangeLayerWeightOverTime(layerIndex, weightIndex,0.5f));
        }

        private IEnumerator ChangeLayerWeightOverTime(int layerIndex, float weightIndex, float duration)
        {
            float elapsedTime = 0f;
            float startWeight = playerAnimator.GetLayerWeight(layerIndex);

            while (elapsedTime < duration)
            {
                float newWeight = Mathf.Lerp(startWeight, weightIndex, elapsedTime / duration);
                playerAnimator.SetLayerWeight(layerIndex, newWeight);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

           
            playerAnimator.SetLayerWeight(layerIndex, weightIndex);
        }

        internal void OnGetPlayerSpeed(float speed)
        {
            _playerSpeed = speed;
            playerAnimator.SetFloat(_speed, speed);
        }

        internal void OnPlayerPressCrouchButton()
        {
            if (!_isCrouching)
            { 
                _isCrouching = !_isCrouching;
                OnChangePlayerAnimationState(PlayerAnimationState.isCrouching, _isCrouching);
               
            }
            else
            {
                _isCrouching = !_isCrouching;
                OnChangePlayerAnimationState(PlayerAnimationState.isCrouching, _isCrouching);
                
            }
        }
        #region Animation Events

        void AnimEventCancelPlayerMovement() => CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.CancelPlayerMovement);
        void AnimEventActivatePlayerMovement() => CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.ActivatePlayerMovement);
         void AnimEventOnCrouchToRunEvent()
         {
            OnChangePlayerAnimationState(PlayerAnimationState.isCrouching,false);
            _isCrouching = false;
         }
         void AnimEventRollEvent()
         {
            InputSignals.Instance.onPlayerIsAvailableForRoll?.Invoke(true);
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationState.isRolling,false);
         }

         void AnimEventStartedAttackEvent()
         {
             CoreGameSignals.Instance.onIsInputReady?.Invoke(false);
           PlayerSignals.Instance.onIsPlayerReadyToPunch?.Invoke(false) ;   
         }

         void AnimEventFinishedAttackEvent()
         {
             CoreGameSignals.Instance.onIsInputReady?.Invoke(true);
             PlayerSignals.Instance.onIsPlayerReadyToPunch?.Invoke(true) ;
             
         }

         void AnimEventPistolHolsterPosition()
         {
             pistol.SetParent(rightHand,false);
         }

         void AnimEventPistolUnHolsterPosition()
         {
             pistol.SetParent(holster,false);
         }

         void AnimEventPistolTakeOut()
         {
            OnChangeAnimationLayerWeight((int)PlayerAnimationLayerEnum.Pistol,1,0);
           
            AnimEventActivatePlayerMovement();
            
         }

         void AnimEventPistolTakeIn()
         {
             OnChangeAnimationLayerWeight((int)PlayerAnimationLayerEnum.Pistol,0,0);
             AnimEventActivatePlayerMovement();
            
         }
         

       

        
        #endregion

       
    }
    
}