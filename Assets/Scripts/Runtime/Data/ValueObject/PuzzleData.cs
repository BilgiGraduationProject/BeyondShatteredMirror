using System;
using System.Collections.Generic;
using Runtime.Enums.Puzzle;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PuzzleData
    {
        public PuzzleEnum puzzleEnum;
        public List<GameObject> puzzlePieces;
        public int countOfPieces;
    }
}