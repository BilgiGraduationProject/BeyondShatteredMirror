using Runtime.Enums.Playable;
using Runtime.Extentions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PlayableSignals : MonoSingleton<PlayableSignals>
    {
        public UnityAction<PlayableEnum> onSetUpCutScene = delegate {  };
      
        
        
    }
}