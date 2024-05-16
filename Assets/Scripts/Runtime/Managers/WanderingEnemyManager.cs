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

       
        [SerializeField] private List<ListOfCheckPoints> checkPointsList;
        #endregion

        #endregion


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onGetCheckPointsList += GetCheckPointsList;
        }

        private List<Transform> GetCheckPointsList(WanderinEnemy type)
        {
            return checkPointsList[(int)type].checkPoints;
        }


        private void UnSubscribeEvents()
        {
           CoreGameSignals.Instance.onGetCheckPointsList -= GetCheckPointsList;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        
        
        [Serializable]
        public struct ListOfCheckPoints{
            public List<Transform> checkPoints;}
    }
}