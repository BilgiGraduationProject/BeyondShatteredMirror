using System;
using Runtime.Controllers.Enemy;
using Runtime.Enums.Playable;
using Runtime.Enums.UI;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Mirror"))
            {
                PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.StandFrontOfMirror);
               PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.StandFrontOfMirror);
               Destroy(other.gameObject);
               
            }
            if (other.CompareTag("House"))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke(2);
                other.CompareTag("Untagged");
            }
            if (other.gameObject.TryGetComponent<EnemyAIController>(out var controller))
            {
                if (!InputSignals.Instance.onGetCombatState()) return;
                controller.TakeDamage(UnityEngine.Random.Range(25f,45f));
                print("Aslan hit Shadow".ColoredText(Color.Lerp(Color.yellow, Color.cyan, 0.5f)));
            }
        }
    }
}