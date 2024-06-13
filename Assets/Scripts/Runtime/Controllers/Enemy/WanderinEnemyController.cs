using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Enums.Enemy;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Runtime.Controllers.Enemy
{
    public class WanderinEnemyController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private WanderinEnemy wanderingEnemies;
        
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private int wayPointIndex;
        [SerializeField] private Animator animator;



        [Header("Limiters")] [SerializeField] private List<Transform> wayPoints;
        

        #region Private Variables;
        private Vector3 target;
        private float elapsedTime;
        

        #endregion

        #endregion

        

        private void Update()
        {
            if (!(Vector3.Distance(transform.position, target) < 1f)) return;
            if(elapsedTime < 4)
            {
                agent.velocity = Vector3.zero;
                animator.SetBool("Walking",false);
                elapsedTime += 1 * Time.deltaTime;
            }
            else
            {
               
                elapsedTime = 0;
                UpdateDestination();
            }
        }

        private void Start()
        {
            var randomIndex = Random.Range(0, wayPoints.Count);
            var newPos = new Vector3( wayPoints[randomIndex].position.x, transform.position.y, wayPoints[randomIndex].position.z);
            transform.position = newPos;
            UpdateDestination();

        }


        private void UpdateDestination()
        {
            animator.SetBool("Walking", true);
            Vector3 newPos;
               var randomIndex = Random.Range(0, wayPoints.Count);
                 newPos = new Vector3( wayPoints[randomIndex].position.x, transform.position.y, wayPoints[randomIndex].position.z);
                 target = newPos;
                 agent.SetDestination(newPos);
               
                
           

                
            
        }


        bool IsPositionOnNavMesh(Vector3 position, float maxDistance)
        {
            NavMeshHit hit;
            bool isOnNavMesh = NavMesh.SamplePosition(position, out hit, maxDistance, NavMesh.AllAreas);
            return isOnNavMesh;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("WanderEnemy"))
            {
                Debug.LogWarning("Enemy  collided with enemy");
                UpdateDestination();
            }

            if (other.CompareTag("Obstacle"))
            {
                Debug.LogWarning("Enemy collied with obstalce");
                UpdateDestination();
            }
        }
    }
}