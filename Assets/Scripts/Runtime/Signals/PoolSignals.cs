using System;
using Runtime.Enums.Pool;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PoolSignals : MonoSingleton<PoolSignals>
    {
        //public Func<PoolType,Transform, GameObject> onGetPoolObject = (_, _) => null;
        public Func<PoolType,Transform, GameObject> onGetPoolObject = delegate { return null; };
        public UnityAction<GameObject,PoolType> onSendPool = delegate {  };
    }
}