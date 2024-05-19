using Runtime.Controllers.Player;
using Runtime.Enums.Player;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Controllers.Enemy
{
    public class EnemyDamageController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField] private BoxCollider collider;

        #endregion

        #region Private Variables

        private float _damage = 8f;
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
            if (_canDealDamage && other.gameObject.TryGetComponent<PlayerPhysicController>(out var physic))
            {
                //other.collider.GetComponent<Player.PlayerDamageController>().TakeDamage(weaponDamage);
                PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Damage);
                PlayerSignals.Instance.onTakeDamage?.Invoke(_damage);
                print("Shadow hit Player".ColoredText(Color.Lerp(Color.yellow, Color.cyan, 0.5f)));
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
