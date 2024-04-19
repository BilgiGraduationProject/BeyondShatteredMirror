using System;
using Runtime.Enums.Collectable;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CollectableSignals : MonoSingleton<CollectableSignals>
    {
        public UnityAction<GameObject,CollectableEnum> onCollectableDoJob = delegate {  };
        public UnityAction<GameObject> onCheckCollectableType = delegate {  };
        public UnityAction<CollectableEnum> onSendCollectableType  = delegate {  };
        public UnityAction<GameObject,bool> onChangeCollectableColor = delegate {  };
    }
}