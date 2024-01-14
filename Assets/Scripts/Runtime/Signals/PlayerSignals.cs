using Runtime.Enums.Player;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
       public UnityAction<PlayerAnimationState,bool> onChangePlayerAnimationState = delegate {  };
       public UnityAction<string> onTriggerPlayerAnimationState = delegate {  };
       public UnityAction<bool> onIsPlayerReadyToPunch = delegate {  };
       public UnityAction<bool> onIsPlayerReadyToShoot = delegate {  };
       public UnityAction<bool> onPlayerCanShoot = delegate {  };
       public UnityAction<int,float,float> onChangeAnimationLayerWeight = delegate {  };
      
      
        
    }
}