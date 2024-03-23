using System;
using Runtime.Enums.Playable;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct CutScenePositionHolderData
    {
        public PlayableEnum playableEnum;
        public Transform cutScenePosition;
        public Vector3 cutSceneRotation;

    }
}