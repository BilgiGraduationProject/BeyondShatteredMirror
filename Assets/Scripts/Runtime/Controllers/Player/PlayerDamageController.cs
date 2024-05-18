using Runtime.Controllers.Enemy;
using Runtime.Enums.Player;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerDamageController : MonoBehaviour
    {
        #region Self Variables

        #region Public  Variables

        //

        #endregion

        #region Serialized Variables

        [SerializeField] private BoxCollider collider;

        #endregion

        #region Private Variables

        private bool canHit = true;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            print(other.name);
            if (other.gameObject.TryGetComponent<EnemyAIController>(out var controller))
            {
                controller.TakeDamage(Random.Range(25f,45f));
                print("Aslan hit Shadow".ColoredText(Color.Lerp(Color.yellow, Color.cyan, 0.5f)));
            }
        }
    }
}