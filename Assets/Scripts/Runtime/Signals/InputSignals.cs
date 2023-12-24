using System;
using System.Numerics;
using Runtime.Extentions;
using Runtime.Keys.Input;
using UnityEngine.Events;
using Vector3 = UnityEngine.Vector3;

namespace Runtime.Signals
{
    public class InputSignals : MonoSingleton<InputSignals>
    { 
        public UnityAction<InputParams> onPlayerPressedMovementButton = delegate {  };
        public UnityAction onPlayerPressedRunButton = delegate {  };
        public UnityAction onPlayerReleaseRunButton = delegate {  };
        public UnityAction onPlayerPressedCrouchButton = delegate {  };
        public UnityAction onPlayerPressedRollButton = delegate {  };
        public UnityAction<bool> onPlayerIsAvailableForRoll = delegate {  };
        public UnityAction onPlayerPressedAttackButton = delegate {  };
        public UnityAction onPlayerPressedCounterButton = delegate {  };
        public UnityAction<bool> onChangeMouseVisibility = delegate {  };
        
    
        
        
       
       
        
        
    }
}