using Runtime.Enums.Enemy;
using Runtime.Enums.Player;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Utilities;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
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
          }

          [Button("Randomize Stats")]
          private void RandomizeStats()
          {
               attackCooldown = Random.Range(2f, 6f);
               attackRange = Random.Range(2f, 4f);
          }
          
          void TakeDamage(float damage)
          {
               health -= damage;
               animator.SetTrigger("Damage");
               
               if(CheckDie())
                    Die();
          }

          void Update()
          {
               if(Input.GetKeyDown(KeyCode.T) && !CheckDie())
               {
                    TakeDamage(50f);
               }

               if (Input.GetKeyDown(KeyCode.Y))
               {
                    
               }
               
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
               Destroy(collider);
               Destroy(hitCollider);
               Destroy(agent);
               Destroy(gameObject, 10f);
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
