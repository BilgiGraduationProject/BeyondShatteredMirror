using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Pool", menuName = "PrototypeData/CD_Pool", order = 0)]
    public class CD_Pool : ScriptableObject
    {
        public List<PoolData> Data;
    }
}