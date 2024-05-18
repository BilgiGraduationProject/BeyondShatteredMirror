using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Controllers.Light
{
    public class FlickerLightController : MonoBehaviour
    {
        #region Self Variables

        #region Serizalized Variables

        [SerializeField] private List<UnityEngine.Light> light;
        [SerializeField] private float blinkTime;
        [Range(0.0f,10f)]
        [SerializeField] private float blinkWaitingTime;
 

        private float timer;



        #endregion

        #endregion


        private void Start()
        {
            StartCoroutine(BlinkLoop());
        }

        private IEnumerator BlinkLoop()
        {
            while (true) // Infinite loop
            {
                yield return StartCoroutine(BlinkLight());
                yield return new WaitForSeconds(blinkTime); // Wait for the specified blink time
            }
        }

        private IEnumerator BlinkLight()
        {
            for (int i = 0; i < light.Count; i++)
            {
                var randomWaitTime = Random.Range(0, blinkWaitingTime);
                light[i].enabled = false;
                yield return new WaitForSeconds(randomWaitTime);
                light[i].enabled = true;

            }
            
        }
    }
}