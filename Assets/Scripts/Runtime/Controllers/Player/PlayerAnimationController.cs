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
        private readonly string _speed = "Speed";

        #endregion

        #endregion
        
        internal void OnChangePlayerAnimationState(PlayerAnimationState animationState, bool condition) =>  
            playerAnimator.SetBool(animationState.ToString(), condition);
        internal void OnTriggerAttackAnimationState(string attackState) => playerAnimator.SetTrigger(attackState);
        internal void OnGetPlayerSpeed(float speed) =>  playerAnimator.SetFloat(_speed,speed);
        
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

        void OnCancelPlayerMovement() => CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StopPlayer);
         void OnCrouchEventUp() => CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StartPlayer);
         void OnCrouchEventDown() => CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StartPlayer);
         void OnCrouchToRunEvent()
         {
            OnChangePlayerAnimationState(PlayerAnimationState.isCrouching,false);
            _isCrouching = false;
         }
         void OnRollEvent()
         {
            InputSignals.Instance.onPlayerIsAvailableForRoll?.Invoke(true);
            PlayerSignals.Instance.onChangePlayerAnimationState?.Invoke(PlayerAnimationState.isRolling,false);
         }

         void OnStartedAttackEvent()
         {
             CoreGameSignals.Instance.onIsInputReady?.Invoke(false);
           PlayerSignals.Instance.onIsPlayerReadyToAttack?.Invoke(false) ;   
         }

         void OnFinishedAttackEvent()
         {
             CoreGameSignals.Instance.onIsInputReady?.Invoke(true);
             PlayerSignals.Instance.onIsPlayerReadyToAttack?.Invoke(true) ;
             
         }

         void ChangePlayerDirection()
         {
             
         }
        #endregion


      
    }
    
}