using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_PlayerPlayable", menuName = "PrototypeData/CD_PlayerPlayable", order = 0)]
    public class CD_PlayerPlayable : ScriptableObject
    {
        public List<PlayerPlayableData> PlayerPlayable;
    }
}