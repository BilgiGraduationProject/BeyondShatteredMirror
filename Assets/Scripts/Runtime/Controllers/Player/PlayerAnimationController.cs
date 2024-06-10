using Runtime.Enums.Player;
using Runtime.Signals;
using UnityEngine;
using DG.Tweening;

namespace Runtime.Controllers.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        [HideInInspector] public float DamageAmountMain = 20f;
        
        #endregion
        
        #region Serialized Variables

        [SerializeField] private Animator _playerAnimator;

        [SerializeField] private AudioClip[] FootstepAudioClips;
        [Range(0, 1)] [SerializeField] private float FootstepAudioVolume = 0.5f;

        #endregion

        #region Private Variables

        private readonly string _speed = "Speed";
        private readonly string _crouch = "Crouch";
        private readonly string _roll = "Roll";
        private CharacterController _characterController;
        
        #endregion

        #endregion

        private void Awake()
        {
            _characterController = GetComponentInParent<CharacterController>();
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
            InputSignals.Instance.onChangeCrouchState?.Invoke(false);
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

        private void OnFootstep(AnimationEvent animationEvent)
        {
            print(animationEvent.animatorClipInfo.weight);
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_characterController.center), FootstepAudioVolume);
                }
            }
        }
        
        public void OnSetAnimationPlayerSpeed(float speed)
        {
            _playerAnimator.SetFloat("Speed",speed);
        }
        
        public void StartDealDamage(int damageType)
        {
            foreach (var controller in GetComponentsInChildren<PlayerDamageController>())
            {
                if (controller.DamageType == damageType)
                {
                    print(_playerAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
                    AnimatorStateInfo stateInfo = _playerAnimator.GetCurrentAnimatorStateInfo(0);
                    print(stateInfo);
                    controller.DamageAmount = DamageAmountMain;
                    controller.StartDealDamage();
                    DOVirtual.DelayedCall(.8f, () => EndDealDamage(damageType));
                }
                else
                {
                    controller.EndDealDamage();
                }
            }
        }
        
        public void EndDealDamage(int damageType)
        {
            foreach (var controller in GetComponentsInChildren<PlayerDamageController>())
            {
                if (controller.DamageType != damageType) continue;
                controller.EndDealDamage();
            }
        }
    }
}