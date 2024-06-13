using DG.Tweening;
using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        #region Self Variables

        #region Public  Variables

        public float Health = 100f;
        public float DamageDealthAmount = 1f;

        #endregion

        #region Serialized Variables

        //

        #endregion

        #region Private Variables

        //
        
        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerSignals.Instance.onTakeDamage += TakeDamage;
            PlayerSignals.Instance.onSetHealthValue += UpdateHealth;
        }

        private void UnsubscribeEvents()
        {
            PlayerSignals.Instance.onTakeDamage -= TakeDamage;
            PlayerSignals.Instance.onSetHealthValue -= UpdateHealth;
        }

        private void Start()
        {
            CoreUISignals.Instance.onSetHealthSlider?.Invoke(Health);
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void TakeDamage(float damage)
        {
            if(Health <= 0) return;
            damage *= DamageDealthAmount;
            Health -= damage;
            CoreUISignals.Instance.onSetHealthSlider?.Invoke(Health);
            print("Player took damage: " + damage);

            if (CheckDie()) Die();
        }

        private void UpdateHealth(float health)
        {
            Health = health;
            CoreUISignals.Instance.onSetHealthSlider?.Invoke(Health);
        }

        private bool CheckDie()
        {
            return Health <= 0;
        }

        private void Die()
        {
            print("Player died");
            if (CoreGameSignals.Instance.onSendCurrentGameStateToUIText?.Invoke() == PlayableEnum.ShowHakan)
            {
                PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Die);
                DOVirtual.DelayedCall(2f, () =>
                {
                    CoreUISignals.Instance.onOpenUnCutScene?.Invoke(PlayableEnum.ShowHakan);
                    Health = 100f;
                    PlayerSignals.Instance.onSetHealthValue?.Invoke(Health);
                    PlayerSignals.Instance.onSetHappinessValue?.Invoke(80);
                
                    InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
                    PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Live);
               
                });
            }
            else
            {
                PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Die);
                DOVirtual.DelayedCall(2f, () =>
                {
                    CoreUISignals.Instance.onOpenUnCutScene?.Invoke(PlayableEnum.PlayerDiedReturnSpawnPoint);
                    Health = 100f;
                    PlayerSignals.Instance.onSetHealthValue?.Invoke(Health);
                    PlayerSignals.Instance.onSetHappinessValue?.Invoke(80);
                
                    InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
                    PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Live);
               
                });
            }
            
            
        }
    }
}