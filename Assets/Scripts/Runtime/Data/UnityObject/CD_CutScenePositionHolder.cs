using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_CutScenePositionHolder", menuName = "PrototypeData/CD_CutScenePositionHolder", order = 0)]
    public class CD_CutScenePositionHolder : ScriptableObject
    {
        public List<CutScenePositionHolderData> cutSceneHolders = new List<CutScenePositionHolderData>();
    }
}