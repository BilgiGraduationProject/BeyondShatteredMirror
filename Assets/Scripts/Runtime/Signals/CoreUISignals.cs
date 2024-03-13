using Runtime.Enums.UI;
using Runtime.Extentions;
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
    }
}