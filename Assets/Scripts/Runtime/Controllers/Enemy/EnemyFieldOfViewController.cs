using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Signals;
using Sirenix.OdinInspector;
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
        [SerializeField] private AudioClip enemyTalkSound;
        [SerializeField] private AudioClip shootSound;
        [SerializeField] private AudioSource audioSource;
        #endregion

        #region Private Variables

        public GameObject player;
        private bool _isPlayerDead;

        #endregion

        #endregion


        private void OnEnable()
        {
            EnemySignals.Instance.onResetEnemy += () =>
            {
                _isPlayerDead = false;
                navMeshAgent.isStopped = false;
            };
        }

       

        private void OnDisable()
        {
            
            EnemySignals.Instance.onResetEnemy += () =>
            {
                _isPlayerDead = false;
                navMeshAgent.isStopped = false;
            };
        }

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
            
            if (_isPlayerDead) return;
            if (rangeChecks.Length != 0)
            {
                Transform target = rangeChecks[0].transform;
                player = target.gameObject;
                var newDirectionTarget = new Vector3(target.position.x - transform.position.x, target.position.y, target.position.z - transform.position.z);

                Debug.LogWarning("Enemy catch the player2");
                if (Vector3.Angle(transform.forward, newDirectionTarget) < angle / 2)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    Debug.DrawRay(transform.position, newDirectionTarget * distanceToTarget, Color.green);
                    if (!Physics.Raycast(transform.position, newDirectionTarget, distanceToTarget, obstructionMask))
                    {
                        
                        navMeshAgent.isStopped = true;
                        enemyTransform.LookAt(player.transform);
                        audioSource.clip = shootSound;
                        audioSource.Play();
                        enemyAnimator.SetTrigger("Shoot");
                        AudioSource.PlayClipAtPoint(shootSound, transform.TransformPoint(target.position), 0.5f);
                        PlayerSignals.Instance.onPlayerDiedOnWanderingEnemy?.Invoke();
                        canSeePlayer = true;
                        _isPlayerDead = true;
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


        
        [Button("Try Sound")]
        private void TrySound()
        {AudioSource.PlayClipAtPoint(shootSound, transform.TransformPoint(enemyTransform.position), 0.5f);
            
        }

       
    }
}