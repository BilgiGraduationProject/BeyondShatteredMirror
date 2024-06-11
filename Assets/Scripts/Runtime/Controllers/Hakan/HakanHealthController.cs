using System;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.Hakan
{
    public class HakanHealthController : MonoBehaviour
    {

        #region Serialized Variables

        [SerializeField] private Slider healthSlider;

        #endregion


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EnemySignals.Instance.onSetHakanHealth += OnSetHakanHealth;
        }

        private void OnSetHakanHealth(float health)
        {
            healthSlider.value = health;
        }


        private void UnSubscribeEvents()
        {
            EnemySignals.Instance.onSetHakanHealth -= OnSetHakanHealth;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}