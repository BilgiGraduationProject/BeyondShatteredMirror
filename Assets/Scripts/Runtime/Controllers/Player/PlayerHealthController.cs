using MoreMountains.Tools;
using Runtime.Signals;
using UnityEditor;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.Controllers.Player
{
    public class PlayerHealthController : MonoBehaviour
    {
        #region Self Variables

        #region Public  Variables

        //

        #endregion

        #region Serialized Variables

        //

        #endregion

        #region Private Variables

        private float _health = 100f;

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PlayerSignals.Instance.onTakeDamage += TakeDamage;
            PlayerSignals.Instance.onSetHealthValue += x => { _health = x;};
        }

        private void UnsubscribeEvents()
        {
            PlayerSignals.Instance.onTakeDamage -= TakeDamage;
            PlayerSignals.Instance.onSetHealthValue -= x => { _health = x;};
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        void TakeDamage(float damage)
        {
            if(_health <= 0) return;
            _health -= damage;
            CoreUISignals.Instance.onSetHealthSlider?.Invoke(_health);
            print("Player took damage: " + damage);

            if (CheckDie()) Die();
        }
        
        bool CheckDie()
        {
            if(_health <= 0) return true;
            return false;
        }
        
        void Die()
        {
            print("Player died");
            //PlayerSignals.Instance.onPlayerDied?.Invoke();
        }
    }
}