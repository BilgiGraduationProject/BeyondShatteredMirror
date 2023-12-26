using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Collectable", menuName = "PrototypeData/CD_Collectable", order = 0)]
    public class CD_Collectable : ScriptableObject
    {
        public List<CollectableData> Data;
    }
}