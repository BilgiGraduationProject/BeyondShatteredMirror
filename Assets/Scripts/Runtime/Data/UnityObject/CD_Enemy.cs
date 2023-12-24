using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Enemy", menuName = "PrototypeData/CD_Enemy", order = 0)]
    public class CD_Enemy : ScriptableObject
    {
        public List<EnemyData> Data;
    }
}