using System.Collections.Generic;
using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Puzzle", menuName = "PrototypeData/CD_Puzzle", order = 0)]
    public class CD_Puzzle : ScriptableObject
    {
        public List<PuzzleData> puzzleSO;
    }
}