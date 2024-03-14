using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
       public UnityAction<float> onGetPlayerSpeed = delegate {  };
       public UnityAction<int> onSetPlayerToCutScenePosition = delegate {  };
      
      
        
    }
}