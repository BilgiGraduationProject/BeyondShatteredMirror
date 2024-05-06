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
                print("Mirror Triggered");
                PlayerSignals.Instance.onSetPlayerToCutScenePosition?.Invoke(PlayableEnum.StandFrontOfMirror);
                UITextSignals.Instance.onChangeMissionText?.Invoke(UITextEnum.FindMemoryCards);
                PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.StandFrontOfMirror);
                other.CompareTag("Untagged");
               
            }

            if (other.CompareTag("House"))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke(2);
                other.CompareTag("Untagged");
            }

          
            

           
        }

       
    }
}