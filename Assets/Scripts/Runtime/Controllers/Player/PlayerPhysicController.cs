using System;
using Runtime.Controllers.Enemy;
using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Enums.UI;
using Runtime.Signals;
using Runtime.Utilities;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicController : MonoBehaviour
    {
        [SerializeField] private CapsuleCollider _crouchCollider;
        [SerializeField] private CapsuleCollider _standCollider;
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
            // if (other.gameObject.TryGetComponent<EnemyAIController>(out var controller))
            // {
            //     if (!InputSignals.Instance.onGetCombatState()) return;
            //     controller.TakeDamage(UnityEngine.Random.Range(25f,45f));
            //     print("Aslan hit Shadow".ColoredText(Color.Lerp(Color.yellow, Color.cyan, 0.5f)));
            // }

            if (other.CompareTag("HakanEntry"))
            {
                PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.ShowHakan);
                Destroy(other.gameObject);
                PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.ShowHakan);
                CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.ShowHakan);
            }


            if (other.CompareTag("HakanLeftHand"))
            {
                Debug.LogWarning("Hakan attacked aslan");
                PlayerSignals.Instance.onSetAnimationTrigger?.Invoke(PlayerAnimationState.Damage);
                PlayerSignals.Instance.onTakeDamage?.Invoke(18f);

            }
        }

        public void OnPlayerCrouch(bool condition)
        {
            if (condition)
            {
                _crouchCollider.enabled = true;
                _standCollider.enabled = false;
            }
            else
            {
                _standCollider.enabled = true;
                _crouchCollider.enabled = false;
            }
        }
    }
}