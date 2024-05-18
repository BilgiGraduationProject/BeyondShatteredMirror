using DG.Tweening;
using Runtime.Enums;
using Runtime.Enums.Enemy;
using Runtime.Enums.Player;
using Runtime.Enums.Pool;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Utilities;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Runtime.Controllers.Enemy
{
     class EnemyAIController : MonoBehaviour
     {
          #region Self Variables

          #region Serialized Varaibles

          [SerializeField] private float health;
          [SerializeField] private Animator animator;
          [SerializeField] private NavMeshAgent agent;
          [SerializeField] private CapsuleCollider collider;
          [SerializeField] private BoxCollider hitCollider;
          [SerializeField] private Slider healthBar;
          [SerializeField] private Image healthBarImageGradient;
          [SerializeField] private Gradient healtGradient;

          [Header("Combat")] 
          [SerializeField] private float attackCooldown = 3f;
          [SerializeField] private float attackRange = 2f;
          [SerializeField] private float aggroRange = 50f;
          
          #endregion

          #region Private Variables

          private GameObject player;
          private float timePassed;
          private float newDestinationCooldown = .5f;
          
          #endregion

          #endregion

          private void Awake()
          {
               GetReferences();
               RandomizeStats();
          }
          
          void GetReferences()
          {
               animator = GetComponent<Animator>();
               agent = GetComponent<NavMeshAgent>();
               collider = GetComponent<CapsuleCollider>();
               player = GameObject.FindGameObjectWithTag("Player");
               healthBar.value = health / 100f;
               healthBarImageGradient.color = healtGradient.Evaluate(health / 100f);
          }

          [Button("Randomize Stats")]
          private void RandomizeStats()
          {
               attackCooldown = Random.Range(2f, 6f);
               attackRange = Random.Range(2f, 4f);
          }
          
          public void TakeDamage(float damage)
          {
               health -= damage;
               healthBar.DOValue(health / 100f, 0.5f);
               healthBarImageGradient.color = healtGradient.Evaluate(health / 100f);
               animator.SetTrigger("Damage");
               
               if(CheckDie())
                    Die();
          }

          void Update()
          {
               if(CheckDie()) return;
               
               animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
               
               if(timePassed >= attackCooldown)
               {
                    if(Vector3.Distance(player.transform.position, transform.position) <= attackRange)
                    {
                         animator.SetTrigger("Attack");
                         timePassed = 0;
                    }
               }
               timePassed += Time.deltaTime;
               
               if(newDestinationCooldown <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
               {
                    newDestinationCooldown = .5f;
                    agent.SetDestination(player.transform.position);
               }
               
               newDestinationCooldown -= Time.deltaTime;
               transform.LookAt(player.transform);
          }

          bool CheckDie()
          {
               if(health <= 0) return true;
               return false;
          }

          void Die()
          {
               print("Enemy Died".ColoredText(Color.Lerp(Color.gray, Color.red, 0.5f)));
               animator.SetTrigger("Die");
               collider.enabled = false;
               Destroy(hitCollider);
               Destroy(agent);
               Destroy(healthBar.gameObject);
               PoolSignals.Instance.onGetPoolObject?.Invoke(PoolType.Soul, gameObject.transform);
               GameDataManager.SaveData<int>(GameDataEnums.Soul.ToString(), GameDataManager.LoadData<int>(GameDataEnums.Soul.ToString(), 0) + 10);
               EnemySignals.Instance.onEnemyDied?.Invoke();
               DOVirtual.DelayedCall(3f ,() =>
               {
                    PoolSignals.Instance.onSendPool?.Invoke(gameObject, PoolType.Enemy);
               });
          }

          public void StartDealDamage()
          {
               GetComponentInChildren<EnemyDamageController>().StartDealDamage();
          }
        
          public void EndDealDamage()
          {
               GetComponentInChildren<EnemyDamageController>().EndDealDamage();
          }
          
          void OnDrawGizmos()
          {
               Gizmos.color = Color.Lerp(Color.red, Color.yellow, 0.5f);
               Gizmos.DrawWireSphere(transform.position, attackRange);
               Gizmos.color = Color.Lerp(Color.yellow, Color.green, 0.5f);
               Gizmos.DrawWireSphere(transform.position, aggroRange);
          }
     }
}
