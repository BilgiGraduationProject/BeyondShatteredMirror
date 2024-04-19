using System;
using System.Collections.Generic;
using Runtime.Enums.Collectable;
using UnityEngine;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class CollectableData
    {
        public List<GameObject> CollectableList = new List<GameObject>();

    }
}