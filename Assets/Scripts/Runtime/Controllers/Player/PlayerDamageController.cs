using Runtime.Controllers.Enemy;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerDamageController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public int DamageType;
        [HideInInspector] 
        public float DamageAmount = 20f;

        #endregion
        
        #region Serialized Variables
        
        [SerializeField] private BoxCollider collider;
        
        #endregion

        #region Private Variables

        private bool _canDealDamage;
        private bool _hasDealtDamage;

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
            _canDealDamage = false;
            _hasDealtDamage = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_canDealDamage && other.gameObject.TryGetComponent<EnemyAIController>(out var controller))
            {
                if (!InputSignals.Instance.onGetCombatState()) return;
                controller.TakeDamage(DamageAmount);
                print("Aslan hit Shadow".ColoredText(Color.Lerp(Color.yellow, Color.cyan, 0.5f)));
                _hasDealtDamage = true;
            }
        }

        public void StartDealDamage()
        {
            if (collider) collider.enabled = true;
            _canDealDamage = true;
            _hasDealtDamage = false;
        }
        
        public void EndDealDamage()
        {
            if (collider is not null) collider.enabled = false;
            _canDealDamage = false;
        }
    }
}