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

        [SerializeField] private float weaponLength;
        [SerializeField] private float weaponDamage;
        [SerializeField] private BoxCollider collider;

        #endregion

        #region Private Variables

        private readonly string _damage = "Damage";
        private bool canDealDamage;
        private bool hasDealtDamage;

        #endregion

        #endregion

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
        
        void Update()
        {
            if (canDealDamage && !hasDealtDamage)
            {
                //DealDamage();
            }
        }

        void DealDamage()
        {
            RaycastHit hit;
            
            int layerMask = 1 << 8;
            
            if (Physics.Raycast(transform.position, transform.up, out hit, weaponLength))
            {
                if (hit.collider.TryGetComponent<PlayerPhysicController>(out var physic))
                {
                    //hit.collider.GetComponent<Player.PlayerDamageController>().TakeDamage(weaponDamage);
                    PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Damage);
                    print("Vuruyo Shadow".ColoredText(Color.Lerp(Color.yellow, Color.cyan, 0.5f)));
                    hasDealtDamage = true;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            print(other.name);
            if (canDealDamage && other.gameObject.TryGetComponent<PlayerPhysicController>(out var physic))
            {
                //other.collider.GetComponent<Player.PlayerDamageController>().TakeDamage(weaponDamage);
                PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Damage);
                print("Vuruyo Shadow".ColoredText(Color.Lerp(Color.yellow, Color.cyan, 0.5f)));
                hasDealtDamage = true;
            }
        }

        public void StartDealDamage()
        {
            collider.enabled = true;
            canDealDamage = true;
            hasDealtDamage = false;
        }
        
        public void EndDealDamage()
        {
            collider.enabled = false;
            canDealDamage = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, transform.up * weaponLength);
        }
    }
}
