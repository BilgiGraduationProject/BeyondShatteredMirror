using System;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PlayerData
    {
        public float PlayerSpeed;
        public float RotationSpeed;
        public float RollForce;
        public float CameraRayDistance;
    }
}