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



        [Header("Limiters")] 
        private   Transform _maxZ;
        private Transform _minZ;
        private Transform _maxX;
        private Transform _minX;
        

        #region Private Variables
       [SerializeField] private List<Transform> checkPointList;
        private Vector3 target;
        private float elapsedTime;
        

        #endregion

        #endregion


        private void OnEnable()
        {
            CoreGameSignals.Instance.onGetCheckPointsList += GetCheckPointsList;
        }

        private void GetCheckPointsList(Transform arg0, Transform arg1, Transform arg2, Transform arg3)
        {
            _maxX = arg0;
            _minX = arg1;
            _maxZ = arg2;
            _minZ = arg3;
        }

        private void OnDisable()
        {
            CoreGameSignals.Instance.onGetCheckPointsList -= GetCheckPointsList;
        }

        private void Update()
        {
            if (!(Vector3.Distance(transform.position, target) < 0.3f)) return;
            if(elapsedTime < 4)
            {
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
            
            var newPos = new Vector3(Random.Range(_maxX.position.x,_minX.position.x), transform.position.y, Random.Range(_maxZ.position.z,_minZ.position.z));
            transform.position = newPos;
            UpdateDestination();

        }


        private void UpdateDestination()
        {
            animator.SetBool("Walking", true);
            Vector3 newPos;
            bool foundValidPosition = false;

            while (!foundValidPosition)
            {
                newPos = new Vector3(Random.Range(_maxX.position.x, _minX.position.x), transform.position.y, Random.Range(_maxZ.position.z, _minZ.position.z));
                if (IsPositionOnNavMesh(newPos, 20f))
                {
                    target = newPos;
                    agent.SetDestination(newPos);
                    foundValidPosition = true;
                }
            }

                
            
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