using Runtime.Enums;
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
        
        public UnityAction<SFXTypes> onPlayMusic = delegate {  };
        public UnityAction onStopMusic = delegate {  };
        public UnityAction<SFXTypes> onPlayOneShotSound = delegate {  };
        public UnityAction<float> onSetMusicVolume = delegate {  };
        public UnityAction<float> onSetSoundVolume = delegate {  };
        
        public UnityAction<PlayableEnum> onOpenUnCutScene = delegate {  };
        public UnityAction<PlayableEnum> onCloseUnCutScene = delegate {  };
        
        public UnityAction<float> onSetHealthSlider = delegate {  };
        public UnityAction<float> onSetHappinesSlider = delegate {  };
        public UnityAction<float, PillTypes> onActivatePill = delegate {  };
        public UnityAction<PillTypes> onPillCollected = delegate {  };
    }
}