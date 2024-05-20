using System;
using Runtime.Enums.Playable;
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
        public Func<Transform> onGetLevelHolderTransform = delegate { return null; };
        public Func<PoolType,Transform,GameObject> onGetLevelHolderPoolObject = delegate { return null; };
        
        public UnityAction<LevelEnum,PlayableEnum> onLoadLevel = delegate {  };
        
        
        public UnityAction onDestroyTheCurrentLevel = delegate {  };
        public UnityAction<bool> onSetAslanHouseVisible = delegate {  };
        
        
        public UnityAction<bool> onSetCurrentLevelToVisible = delegate  {  };
        public UnityAction onDestroyFightLevel = delegate {  };
    }
}