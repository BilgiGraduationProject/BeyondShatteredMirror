using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PlayerData
    {
        public float PlayerSpeed;
        public float RotationSpeed;
        public float RollForce;
        public float CameraRayDistance;
        public float PlayerSpeedMultiplier;


        
    }
}