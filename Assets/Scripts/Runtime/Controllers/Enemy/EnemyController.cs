using System;
using System.Collections;
using System.Collections.Generic;
using Runtime.Data.ValueObject;
using Runtime.Signals;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Controllers.Enemy
{
    public class EnemyController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

         private float _enemyHealth;

        #endregion

        #region Private Variables
        
        

        #endregion

        #endregion

        
        internal float GetEnemyHealth() => _enemyHealth;
        
        internal void GetEnemyData(EnemyData enemyData)
        {
            _enemyHealth = enemyData.EnemyHealth;
        }
    }
}