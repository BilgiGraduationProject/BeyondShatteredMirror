﻿using DG.Tweening;
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
        
        void TakeDamage(float damage)
        {
            if(Health <= 0) return;
            damage *= DamageDealthAmount;
            Health -= damage;
            CoreUISignals.Instance.onSetHealthSlider?.Invoke(Health);
            print("Player took damage: " + damage);

            if (CheckDie()) Die();
        }
        
        void UpdateHealth(float health)
        {
            Health = health;
            CoreUISignals.Instance.onSetHealthSlider?.Invoke(Health);
        }
        
        bool CheckDie()
        {
            if(Health <= 0) return true;
            return false;
        }
        
        void Die()
        {
            print("Player died");
            PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Die);
            DOVirtual.DelayedCall(2f, () =>
            {
                CoreUISignals.Instance.onOpenUnCutScene?.Invoke(PlayableEnum.PlayerDiedReturnSpawnPoint);
                PlayerSignals.Instance.onSetHealthValue?.Invoke(100f);
                Health = 100f;
                CoreUISignals.Instance.onSetHealthSlider?.Invoke(Health);
                CoreUISignals.Instance.onSetHappinesSlider?.Invoke(0f);
                
                InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
                PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Live);
               
            });
            
        }
    }
}