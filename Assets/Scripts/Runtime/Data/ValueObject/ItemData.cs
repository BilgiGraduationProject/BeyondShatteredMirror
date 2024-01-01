using System;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct ItemData
    {
        public string Name;
        public string Description;
        public Sprite Thumbnail;
        public int Price;
    }
}
