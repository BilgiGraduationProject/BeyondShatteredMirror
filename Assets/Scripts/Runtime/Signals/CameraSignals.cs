using Runtime.Enums.Camera;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CameraSignals : MonoSingleton<CameraSignals>
    {
        
        public UnityAction onSetCinemachineTarget = delegate {  };
        public UnityAction<CameraStateEnum> onChangeCameraState = delegate {  };
        
        
        
        
        
    }
    
    
}