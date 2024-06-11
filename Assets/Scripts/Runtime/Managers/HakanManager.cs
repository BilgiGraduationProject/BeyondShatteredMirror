using System;
using DG.Tweening;
using ES3Types;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Runtime.Managers
{
    public class HakanManager : MonoBehaviour
    {

        #region Self Variables

        #region Serialized Variables

        [SerializeField] private NavMeshAgent hakanAgent;
        [SerializeField] private Animator animator;
        [SerializeField] private int hakanHealth = 100;

        #endregion

        #region Private Variables

        private GameObject player;
        private bool _chaseThePlayer;
        private bool IsReadyToAttack = true;
        private bool _isAttacking;
        private bool _isRoaring;
        private bool _isTakingDamage;
        private int stage = 1;
        private bool _isFirstDie;
        private bool _startedToChase;
        #endregion

        #endregion


        private void FixedUpdate()
        {
            ChaseThePlayer();
        }

        private void ChaseThePlayer()
        {
            if (!_chaseThePlayer || _isTakingDamage || _isFirstDie) return;
            switch (stage)
            {
                case 1:
                    AttackThePlayer();
                    break;
                case 2:
                    CaseThePlayer();
                    break;
            }
           

            
            
        }

        private void CaseThePlayer()
        {
            if (_startedToChase) return;
            _startedToChase = true;
            animator.SetBool("Run",true);
            transform.LookAt(player.transform);
            hakanAgent.SetDestination(transform.forward);

        }

        private void AttackThePlayer()
        {
            if(Vector3.Distance(player.transform.position,hakanAgent.transform.position) < 3f)
            {
                if (!IsReadyToAttack) return;
                hakanAgent.velocity = Vector3.zero;
                hakanAgent.isStopped = true;
                transform.LookAt(player.transform);
                animator.SetBool("Run",false);
                animator.SetBool("Attack",true);
            }
            else if (Vector3.Distance(player.transform.position, hakanAgent.transform.position) < 2f && !IsReadyToAttack)
            {
                hakanAgent.velocity = Vector3.zero;
                hakanAgent.isStopped = true;
                transform.LookAt(player.transform);
                animator.SetBool("Run",false);
                animator.SetBool("Attack",false);
                
            }
            else
            {
                if (_isAttacking) return;
                
                if (Vector3.Distance(player.transform.position, hakanAgent.transform.position) > 7f)
                {
                    if (_isRoaring) return;
                    _isAttacking = false;
                    hakanAgent.velocity = Vector3.zero;
                    hakanAgent.isStopped = true;
                    animator.SetTrigger("Roar");
                }
                else if (Vector3.Distance(player.transform.position, hakanAgent.transform.position) < 7f)
                {
                    animator.SetBool("Run",true);
                    hakanAgent.isStopped = false;
                    hakanAgent.SetDestination(player.transform.position);
                }
            }
        }

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EnemySignals.Instance.onStartHakanRun += OnStartHakanRun;
            EnemySignals.Instance.onHakanIsAttacking += OnHakanIsAttacking;
            EnemySignals.Instance.onHakanIsReadyToAttack += OnHakanIsReadyToAttack;
            EnemySignals.Instance.onHakanIsRoaring += OnHakanIsRoaring;
            EnemySignals.Instance.onSetSecondStageForHakan += OnSetSecondStageForHakan;
            EnemySignals.Instance.onHakanFirstDie += OnHakanFirstDie;
        }

        private void OnHakanIsRoaring(bool arg0) => _isRoaring = arg0;

        private void OnHakanIsReadyToAttack(bool arg0) => IsReadyToAttack = arg0;
        

        private void OnHakanIsAttacking(bool arg0) => _isAttacking = arg0;
        

        private void OnStartHakanRun()
        {

            DOVirtual.DelayedCall(2f, () =>
            {
                _chaseThePlayer = true;

            });

        }

        private void UnSubscribeEvents()
        {
            EnemySignals.Instance.onStartHakanRun -= OnStartHakanRun;
            EnemySignals.Instance.onHakanIsAttacking -= OnHakanIsAttacking;
            EnemySignals.Instance.onHakanIsReadyToAttack -= OnHakanIsReadyToAttack;
            EnemySignals.Instance.onHakanIsRoaring -= OnHakanIsRoaring;
            EnemySignals.Instance.onSetSecondStageForHakan -= OnSetSecondStageForHakan;
            EnemySignals.Instance.onHakanFirstDie -= OnHakanFirstDie;

        }

        private void OnHakanFirstDie(bool condition) => _isFirstDie = condition;
        
        private void OnSetSecondStageForHakan()
        {
            stage = 2;
            hakanHealth = 100;
            EnemySignals.Instance.onSetHakanHealth?.Invoke(hakanHealth);
            animator.SetTrigger("GetUp");
            
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("AslanLeftHand"))
            {
                TakeDamage();
            }
            
            if(other.CompareTag("Wall"))
            {
                hakanAgent.velocity = Vector3.zero;
                hakanAgent.isStopped = true;
                _startedToChase = false;
            }
            
            
        }

        private void TakeDamage()
        {
            if (_isAttacking || _isFirstDie) return;
            var randomChange = Random.Range(0, 2);
            var randomReaction = Random.Range(0, 2);
            if (randomChange == 1)
            {
                animator.SetTrigger("Dodge");
            }
            else
            {
                switch (stage)
                {
                    case 1:
                        hakanHealth -= 10;
                        EnemySignals.Instance.onSetHakanHealth?.Invoke(hakanHealth);
                        if (hakanHealth <= 0)
                        {
                            animator.SetTrigger("FirstDie");
                            _isFirstDie = true;
                            EnemySignals.Instance.onFirstDieOfHakanForSlider?.Invoke();
                        }
                        else
                        {
                            animator.SetFloat("Reaction",randomReaction);
                            animator.SetTrigger("Damage");
                        }
                        break;
                    case 2:
                        break;
                    
                }
            }
        }
            
            
        
    }
}