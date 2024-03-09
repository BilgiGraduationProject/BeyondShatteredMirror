using System;
using Runtime.Enums.Playable;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct CutScenePositionHolderData
    {
        public PlayableEnum playableEnum;
        public Vector3 cutScenePosition;

    }
}