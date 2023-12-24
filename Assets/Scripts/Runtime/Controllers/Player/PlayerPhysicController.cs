using System;
using Runtime.Enums.Enemy;
using Runtime.Enums.Player;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicController : MonoBehaviour
    {
        #region Self Variables

        [SerializeField] private PlayerBodyType playerBodyType;

        #endregion

        #region Private Variables

        private readonly string _enemyHead = "EnemyHead";
        private readonly string _enemyBody = "EnemyBody";

        #endregion
        


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_enemyHead))
            {
                switch (playerBodyType)
                {
                    case PlayerBodyType.RightHand:
                       EnemySignals.Instance.onChangeEnemyAnimationState?.Invoke(EnemyAnimationState.FaceHit);
                        break;
                    case PlayerBodyType.LeftHand:
                        EnemySignals.Instance.onChangeEnemyAnimationState?.Invoke(EnemyAnimationState.FaceHit);
                        break;
                    
                }
            }

            else if (other.CompareTag(_enemyBody))
            {
                Debug.LogWarning("Executed");
                switch (playerBodyType)
                {
                    case PlayerBodyType.RightFoot:
                        EnemySignals.Instance.onChangeEnemyAnimationState?.Invoke(EnemyAnimationState.KickBodyHit);
                        EnemySignals.Instance.onAddEnemyToForce?.Invoke(5);
                        break;
                    case PlayerBodyType.LeftFoot:
                        EnemySignals.Instance.onChangeEnemyAnimationState?.Invoke(EnemyAnimationState.KickBodyHit);
                        EnemySignals.Instance.onAddEnemyToForce?.Invoke(5);
                        break;
                    case PlayerBodyType.RightHand:
                        EnemySignals.Instance.onChangeEnemyAnimationState?.Invoke(EnemyAnimationState.PunchBodyHit);
                        EnemySignals.Instance.onAddEnemyToForce?.Invoke(2);
                        break;
                    case PlayerBodyType.LeftHand:
                        EnemySignals.Instance.onChangeEnemyAnimationState?.Invoke(EnemyAnimationState.PunchBodyHit);
                        EnemySignals.Instance.onAddEnemyToForce?.Invoke(2);
                        break;
                    
                }
            }
        }
    }
}