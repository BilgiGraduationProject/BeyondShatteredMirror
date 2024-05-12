using Runtime.Enums.Playable;
using Runtime.Enums.UI;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreUISignals : MonoSingleton<CoreUISignals>
    {
        public UnityAction<UIPanelTypes,short> onOpenPanel = delegate {  };
        public UnityAction<int> onClosePanel = delegate{  };
        public UnityAction onCloseAllPanels = delegate {  };
        public UnityAction onEnableAllPanels = delegate {  };
        public UnityAction onDisableAllPanels = delegate {  };
        public UnityAction<int> onOpenCutscene = delegate {  };
        
        public UnityAction<SFXTypes> onPlaySFX = delegate {  };
        public UnityAction onStopSFX = delegate {  };
        public UnityAction<SFXTypes> onPlayOneShotSFX = delegate {  };
        public UnityAction<float> onSetSFXVolume = delegate {  };
        
        
        public UnityAction onOpenUnCutScene = delegate {  };
        public UnityAction<PlayableEnum> onCloseUnCutScene = delegate {  };
        
        
    }
}