using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_UIMissionText", menuName = "PrototypeData/CD_UIMissionText", order = 0)]
    public class CD_UIMissionTextData : ScriptableObject
    {
        public List<UIMissionTextData> data;
    }
}