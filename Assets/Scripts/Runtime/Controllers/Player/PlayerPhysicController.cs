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
                other.CompareTag("Untagged");
            }

            if (other.CompareTag("House"))
            {
                CoreUISignals.Instance.onOpenCutscene?.Invoke((2));
                other.CompareTag("Untagged");
            }
        }
    }
}