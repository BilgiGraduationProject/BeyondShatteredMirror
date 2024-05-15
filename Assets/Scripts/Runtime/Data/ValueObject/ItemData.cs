using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct ItemData
    {
        public string Name;
        public GameDataEnums DataType;
        public string Description;
        public Sprite Thumbnail;
        public int Price;
    }
}
