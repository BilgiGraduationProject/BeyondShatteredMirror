using Runtime.Data.UnityObject;
using UnityEngine;

namespace Runtime.Commands.Pool
{
    public struct PoolResetCommand
    {
        private readonly CD_Pool _poolData;
        private readonly Transform _poolHolder;
        private readonly GameObject _levelHolder;
        public PoolResetCommand(ref CD_Pool poolData, ref Transform poolHolder, ref GameObject levelHolder)
        {
            _poolData = poolData;
            _poolHolder = poolHolder;
            _levelHolder = levelHolder;
        }

        public void Execute()
        {
            var poolList = _poolData.Data;

            for (var i = 0; i < poolList.Count; i++)
            {
                var child = _poolHolder.GetChild(i);
                if (child.transform.childCount > poolList[i].ObjectCount)
                {
                    var count = child.transform.childCount;
                    for (var j = poolList[i].ObjectCount; j < count; j++)
                    {
                        child.GetChild(poolList[i].ObjectCount).SetParent(_levelHolder.transform.GetChild(0));
                    }
                }

            }
        }
    }
}