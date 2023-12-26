using Runtime.Data.UnityObject;
using UnityEngine;

namespace Runtime.Commands.Collectable
{
    public struct CollectableCreateHolderCommand
    {
        private readonly Transform _collectableHolder;
        private  GameObject _emptyObject;
        private readonly CD_Collectable _collectableData;
        public CollectableCreateHolderCommand(ref Transform collectableHolder, ref GameObject emptyObject, CD_Collectable collectableData)
        {
            _collectableHolder = collectableHolder;
            _emptyObject = emptyObject;
            _collectableData = collectableData;
        }

        public void Execute()
        {
            for (int i = 0; i < _collectableData.Data.Count; i++)
            {
                var colData = _collectableData.Data[i];
                colData.CollectableList.Clear();
                colData.CollectableList.TrimExcess();
                _emptyObject = new GameObject();
                _emptyObject.transform.parent = _collectableHolder;
                _emptyObject.name = colData.CollectableType.ToString();
                colData.CollectableList.Add(_emptyObject);
              

            }
        }
    }
}