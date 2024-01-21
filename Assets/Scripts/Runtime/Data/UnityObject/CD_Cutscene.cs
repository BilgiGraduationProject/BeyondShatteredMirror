using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Cutscene", menuName = "PrototypeData/CD_Cutscene", order = 0)]
    public class CD_Cutscene : ScriptableObject
    {
        public List<CutsceneData> cutsceneDataList = new List<CutsceneData>();
    }
}