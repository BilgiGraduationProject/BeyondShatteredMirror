using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Enums.Enemy;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Controllers.Enemy
{
    public class WanderinEnemyController : MonoBehaviour
    {
        #region Serialized Variables

        [SerializeField] private WanderinEnemy wanderingEnemies;
        
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private int wayPointIndex;
        [SerializeField] private Animator animator;

        #region Private Variables
       [SerializeField] private List<Transform> checkPointList;
        private Vector3 target;
        private float elapsedTime;
        

        #endregion

        #endregion


        private void Update()
        {
            if (!(Vector3.Distance(transform.position, target) < 0.6f)) return;
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
            var pointList = CoreGameSignals.Instance.onGetCheckPointsList?.Invoke(wanderingEnemies);
            if (pointList == null) return;
            Debug.LogWarning(pointList.Count);
            transform.position = pointList[0].position;
            target = pointList[0].position;
            checkPointList = pointList;
            UpdateDestination();

        }


        private void UpdateDestination()
        {
            animator.SetBool("Walking",true);
            wayPointIndex++;
            IterateWayPointIndex();
            target = checkPointList[wayPointIndex].position;
            agent.SetDestination(target);
            
        }


        private void IterateWayPointIndex()
        {
            if(wayPointIndex == checkPointList.Count)
                wayPointIndex = 0;
            
        }


        
    }
}