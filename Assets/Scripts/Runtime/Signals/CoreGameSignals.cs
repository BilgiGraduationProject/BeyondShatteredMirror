using Runtime.Enums.GameManager;
using Runtime.Extentions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction<GameStateEnum> onGameStatusChanged = delegate {  };
        public UnityAction<bool> onIsPlayerReadyToMove = delegate {  };
        public UnityAction<bool> onIsInputReady = delegate {  };
    }
}