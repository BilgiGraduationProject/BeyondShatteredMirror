using System;
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
        [SerializeField] private int wayPointIndex = 1;

        #region Private Variables
        private List<Transform> checkPointList;
        private Vector3 target;
        

        #endregion

        #endregion


        private void Update()
        {
            if (Vector3.Distance(transform.position, target) < 1)
            {
                Debug.LogWarning("New Target Destination");
                IterateWayPointIndex();
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
           
        }


        private void UpdateDestination()
        {
            Debug.LogWarning("New Target Destination");
            target = checkPointList[wayPointIndex].position;
            agent.SetDestination(target);
        }


        private void IterateWayPointIndex()
        {
            wayPointIndex++;
            if(wayPointIndex == checkPointList.Count)
                wayPointIndex = 0;
            
        }
    }
}