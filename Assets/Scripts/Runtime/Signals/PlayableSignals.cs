﻿using Runtime.Enums.Playable;
using Runtime.Extentions;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace Runtime.Signals
{
    public class PlayableSignals : MonoSingleton<PlayableSignals>
    {
        public UnityAction<int> onSetUpCutScene = delegate {  };
        public UnityAction<bool,int> onSendInputManagerToReadyForInput = delegate {  };
      
        
        
    }
}