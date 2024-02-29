using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PlayerData
    {
        public float PlayerSpeed;
        public float RotationSpeed;
        public float RollDistance;
        public float RollTime;
        public float CameraRayDistance;
        public float PlayerSpeedMultiplier;


        
    }
}