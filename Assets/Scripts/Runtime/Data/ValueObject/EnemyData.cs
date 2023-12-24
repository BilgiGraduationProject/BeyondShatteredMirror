using System;
using Runtime.Enums.Enemy;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct EnemyData
    {
        public EnemyType EnemyTypeData;
        public float EnemyHealth;

    }
}