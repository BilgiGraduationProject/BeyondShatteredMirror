using System;
using System.Collections.Generic;
using Runtime.Enums.Enemy;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class WanderingEnemyManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

       
        [SerializeField] private Transform maxX;
        [SerializeField] private Transform minX;
        [SerializeField] private Transform maxZ;
        [SerializeField] private Transform minZ;
        
        #endregion

        #endregion


        private void Awake()
        {
            CoreGameSignals.Instance.onGetCheckPointsList?.Invoke(maxX,minX,maxZ,minZ);
        }
    }
}