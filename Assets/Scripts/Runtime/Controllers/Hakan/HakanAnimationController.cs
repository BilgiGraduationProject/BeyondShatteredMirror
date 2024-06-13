using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Hakan
{
    public class HakanAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private BoxCollider leftHandHakan;
        public void OnAttack()
        {
            EnemySignals.Instance.onHakanIsReadyToAttack?.Invoke(false);
            EnemySignals.Instance.onHakanIsAttacking?.Invoke(true);
            print("Hakan Attack");
            leftHandHakan.enabled = true;
        }

        public void OnEndAttack()
        {
            EnemySignals.Instance.onHakanIsAttacking?.Invoke(false);
            _animator.SetBool("Attack", false);
            DOVirtual.DelayedCall(4f, ()=>
            {
                EnemySignals.Instance.onHakanIsReadyToAttack?.Invoke(true);
            });
            leftHandHakan.enabled = false;
            
        }


        public void OnRoar()
        {
            EnemySignals.Instance.onHakanIsRoaring?.Invoke(true);
            StartCoroutine(WaitTimeOfRoar());
        }

        private IEnumerator WaitTimeOfRoar()
        {
            yield return new WaitForSeconds(20f);
            EnemySignals.Instance.onHakanIsRoaring?.Invoke(false);
           
        }
        
        
        public void OnTakeDamage()
        {
            EnemySignals.Instance.onIsTakingDamage?.Invoke(false);
        }


        public void OnGetUp()
        {
            DOVirtual.DelayedCall(1.5f, () =>
            {
                Debug.LogWarning("Hakan stage 2 is started");
                EnemySignals.Instance.onHakanFirstDie?.Invoke(false);
                EnemySignals.Instance.onIsTakingDamage?.Invoke(false);
            });
        }


        public void OnDodge()
        {
            EnemySignals.Instance.onHakanIsAttacking?.Invoke(true);
            _animator.SetTrigger("Attack");
           _animator.SetBool("isDodge",false);
        }
    }
}