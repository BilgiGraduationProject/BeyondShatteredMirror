using Runtime.Controllers.Enemy;
using Runtime.Enums.Player;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerDamageController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private BoxCollider collider;

        #endregion

        #region Private Variables

        private bool canDealDamage;
        private bool hasDealtDamage;

        #endregion

        #endregion

        private void Awake()
        {
            GetReferences();
        }
        
        void GetReferences()
        {
            collider = GetComponent<BoxCollider>();
        }
        
        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            collider.enabled = false;
            canDealDamage = false;
            hasDealtDamage = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (canDealDamage && other.gameObject.TryGetComponent<EnemyAIController>(out var controller))
            {
                if (!InputSignals.Instance.onGetCombatState()) return;
                controller.TakeDamage(Random.Range(20f,30f));
                print("Aslan hit Shadow".ColoredText(Color.Lerp(Color.yellow, Color.cyan, 0.5f)));
                hasDealtDamage = true;
            }
        }

        public void StartDealDamage()
        {
            if (collider) collider.enabled = true;
            canDealDamage = true;
            hasDealtDamage = false;
        }
        
        public void EndDealDamage()
        {
            if (collider is not null) collider.enabled = false;
            canDealDamage = false;
        }
    }
}