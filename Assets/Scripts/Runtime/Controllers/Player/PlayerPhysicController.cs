using System;
using Runtime.Enums.Playable;
using Runtime.Enums.UI;
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
                PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.StandFrontOfMirror);
               PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.StandFrontOfMirror);
               Destroy(other.gameObject);
               
            }

            if (other.CompareTag("House"))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke(2);
                other.CompareTag("Untagged");
            }
            

           
        }
        
    }
}