using Runtime.Enums.UI;
using Runtime.Extentions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class UITextSignals : MonoSingleton<UITextSignals>
    {
        public UnityAction<UITextEnum> onChangeMissionText;

    }
}