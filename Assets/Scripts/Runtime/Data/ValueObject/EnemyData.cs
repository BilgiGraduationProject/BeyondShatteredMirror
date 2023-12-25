using System;
using Runtime.Enums.Enemy;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct EnemyData
    {
        public EnemyType EnemyTypeData;
        public float EnemyHealth;

        [Header("Enemy Force Type")] 
        public float PunchForce;
        public float KickForce;
        public float FaceForce;
        
        
     [Header("Enemy Damage Type")] 
        public float PunchHitDamage;
        public float KickHitDamage;
         public float FaceHitDamage;

    }
}