using Runtime.Enums.Enemy;
using UnityEngine;

namespace Runtime.Controllers.Enemy
{
    public class EnemyAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Varaibles

        [SerializeField] private Animator enemyAnimator;

        #endregion

        #endregion
        public void ChangeEnemyAnimationState(EnemyAnimationState enemyAnimationState)
        {
            enemyAnimator.SetTrigger(enemyAnimationState.ToString());
        }
    }
}