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
            PlayerSignals.Instance.onKillDamage += KillDamage;
            PlayerSignals.Instance.onSetHappinessValue += UpdateHappiness;
        }

        private void UnsubscribeEvents()
        {
            PlayerSignals.Instance.onKillDamage -= KillDamage;
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

        private void KillDamage(float hitDamage)
        {
            if(Happiness <= 0) return;
            hitDamage *= DamageDealthAmount;
            Happiness -= hitDamage;
            CoreUISignals.Instance.onSetHappinesSlider?.Invoke(Happiness);
            print("Player killed by hit: " + hitDamage * DamageDealthAmount);

            if (CheckZero()) Event();
        }

        private void UpdateHappiness(float happiness)
        {
            Happiness = happiness;
            CoreUISignals.Instance.onSetHappinesSlider?.Invoke(Happiness);
        }

        private bool CheckZero()
        {
            return Happiness <= 0;
        }

        private static void Event()
        {
            
        }
    }
}