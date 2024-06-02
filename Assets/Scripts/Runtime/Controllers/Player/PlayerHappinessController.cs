using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerHappinessController : MonoBehaviour
    {
        #region Self Variables

        #region Public  Variables

        public float Happiness = 100f;
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
            PlayerSignals.Instance.onHitDamage += HitDamage;
            PlayerSignals.Instance.onSetHappinessValue += UpdateHappiness;
        }

        private void UnsubscribeEvents()
        {
            PlayerSignals.Instance.onHitDamage -= HitDamage;
            PlayerSignals.Instance.onSetHappinessValue -= UpdateHappiness;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void Start()
        {
            CoreUISignals.Instance.onSetHappinesSlider?.Invoke(Happiness);
        }
        
        void HitDamage(float hitDamage)
        {
            if(Happiness <= 0) return;
            hitDamage *= DamageDealthAmount;
            Happiness -= hitDamage;
            CoreUISignals.Instance.onSetHappinesSlider?.Invoke(Happiness);
            print("Player killed by hit: " + hitDamage);

            if (CheckZero()) Event();
        }
        
        void UpdateHappiness(float happiness)
        {
            Happiness = happiness;
            CoreUISignals.Instance.onSetHappinesSlider?.Invoke(Happiness);
        }
        
        bool CheckZero()
        {
            if(Happiness <= 0) return true;
            return false;
        }
        
        void Event()
        {
            
        }
    }
}