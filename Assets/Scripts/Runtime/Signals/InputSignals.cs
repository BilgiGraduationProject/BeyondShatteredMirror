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
      
        public UnityAction<bool> onChangeMouseVisibility = delegate {  };
        public UnityAction<bool> onIsPlayerReadyToMove = delegate {  };
        public UnityAction<bool> onPlayerPressedLeftControlButton = delegate {  };
        public UnityAction onPlayerPressedSpaceButton = delegate {  };
        public UnityAction<bool> onIsReadyForCombat = delegate {  };
        public UnityAction onPlayerPressedPickUpButton = delegate {  };
        public UnityAction onPlayerPressedDropItemButton = delegate {  };
      
        
    
        
        
       
       
        
        
    }
}