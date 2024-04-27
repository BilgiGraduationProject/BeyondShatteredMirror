using System;
using Runtime.Enums.Playable;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicController : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Mirror"))
            {
                print("Mirror Triggered");
                other.CompareTag("Untagged");
                PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.StandFrontOfMirror);
                PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.StandFrontOfMirror);
            }

            if (other.CompareTag("House"))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke(2);
                other.CompareTag("Untagged");
            }

            if (other.CompareTag("Ground"))
            {
                PlayerSignals.Instance.onIsPlayerFalling?.Invoke(false);
            }
            
            

           
        }


        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Obstacle") || other.CompareTag("Wall"))
            {
                if (!PlayerSignals.Instance.onIsKillRoll.Invoke()) return;
                Debug.LogWarning("Colliding Role");
                PlayerSignals.Instance.onPlayerCollidedWithObstacle?.Invoke(transform.parent.gameObject.transform);
            }

            
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ground") )
            {
               
                PlayerSignals.Instance.onIsPlayerFalling?.Invoke(true);
                Debug.LogWarning("Exited Ground");
            }
        }
    }
}