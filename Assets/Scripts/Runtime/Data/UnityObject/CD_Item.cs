using Runtime.Data.ValueObject;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Item", menuName = "PrototypeData/CD_Item", order = 0)]
    public class CD_Item : ScriptableObject
    {
        public ItemData Data;
    }
}