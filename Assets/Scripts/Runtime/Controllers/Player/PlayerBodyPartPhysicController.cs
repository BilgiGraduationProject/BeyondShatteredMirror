
using Runtime.Enums.Enemy;
using Runtime.Enums.Player;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerBodyPartPhysicController : MonoBehaviour
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
                       EnemySignals.Instance.onPlayerBodyCollidedWithEnemey?.Invoke(EnemyAnimationState.FaceHit,other.transform.root.gameObject);
                        break;
                    case PlayerBodyType.LeftHand:
                        EnemySignals.Instance.onPlayerBodyCollidedWithEnemey?.Invoke(EnemyAnimationState.FaceHit,other.transform.root.gameObject);
                        break;
                }
            }

            else if (other.CompareTag(_enemyBody))
            {
                
                switch (playerBodyType)
                {
                    case PlayerBodyType.RightFoot:
                       
                        EnemySignals.Instance.onPlayerBodyCollidedWithEnemey?.Invoke(EnemyAnimationState.KickBodyHit,other.transform.root.gameObject);
                        break;
                    case PlayerBodyType.LeftFoot:
                        EnemySignals.Instance.onPlayerBodyCollidedWithEnemey?.Invoke(EnemyAnimationState.KickBodyHit,other.transform.root.gameObject);
                        break;
                    case PlayerBodyType.RightHand:
                        EnemySignals.Instance.onPlayerBodyCollidedWithEnemey?.Invoke(EnemyAnimationState.PunchBodyHit,other.transform.root.gameObject);
                        break;
                    case PlayerBodyType.LeftHand:
                        EnemySignals.Instance.onPlayerBodyCollidedWithEnemey?.Invoke(EnemyAnimationState.PunchBodyHit,other.transform.root.gameObject);
                        break;
                }
            }

            
        }
        
      
    }
}