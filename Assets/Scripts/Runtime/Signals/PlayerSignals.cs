using Runtime.Enums.Player;
using Runtime.Extentions;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
       public UnityAction<PlayerAnimationState,bool> onChangePlayerAnimationState = delegate {  };
       public UnityAction<string> onTriggerAttackAnimationState = delegate {  };
       public UnityAction<bool> onIsPlayerReadyToAttack = delegate {  };
      
        
    }
}