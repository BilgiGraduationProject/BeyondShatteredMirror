using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PoolData
    {
        public string ObjName;
        public GameObject ObjPrefab;
        public int ObjectCount;

    }
}