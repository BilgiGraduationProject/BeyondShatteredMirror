using System;
using Runtime.Enums.Pool;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PoolSignals : MonoSingleton<PoolSignals>
    {
        public Func<PoolType,GameObject> onGetPoolObject = delegate { return null; };
        public UnityAction<GameObject,PoolType> onSendPool = delegate {  };
        
    }
}