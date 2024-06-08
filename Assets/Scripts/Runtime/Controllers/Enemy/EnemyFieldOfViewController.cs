using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime.Controllers.Enemy
{
    public class EnemyFieldOfViewController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] public float radius;
        [Range(0,360)]
        [SerializeField] public float angle;

        [SerializeField] private LayerMask targetMask;
        [SerializeField] private LayerMask obstructionMask;

        [SerializeField] public bool canSeePlayer;
        [SerializeField] private Transform enemyTransform;

        [SerializeField] private Animator enemyAnimator;
        [SerializeField] private NavMeshAgent navMeshAgent;
        #endregion

        #region Private Variables

        public GameObject player;

        #endregion

        #endregion


        private void Start()
        {
            
            StartCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            while (true)
            {   
                yield return wait;
                FieldOfViewCheck();
                
            }
        }

        private void FieldOfViewCheck()
        {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                player = target.gameObject;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        Debug.LogWarning("Enemy catch the player");
                        
                        navMeshAgent.isStopped = true;
                        enemyTransform.LookAt(player.transform);
                        enemyAnimator.SetTrigger("Shoot");
                        canSeePlayer = true;
                    }
                    else
                    {
                        canSeePlayer = false;
                    }
                }
                else if (canSeePlayer)
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
    }
}