using System;
using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
       public UnityAction<float> onGetPlayerSpeed = delegate {  };
       public UnityAction<PlayableEnum> onSetPlayerToCutScenePosition = delegate {  };
       public UnityAction<PlayerAnimationState,bool> onSetAnimationBool = delegate {  };
       public UnityAction<PlayerAnimationState> onSetAnimationTrigger = delegate {  };
       public UnityAction<float> onSetCombatCount = delegate {  };
      
       
       public UnityAction<Transform> onPlayerCollidedWithObstacle = delegate {  };
       
       
       public UnityAction<bool> onIsPlayerFalling = delegate {  };
       
       public Func<bool> onIsKillRoll = delegate { return false; };
       
       
       public UnityAction onPlayerInterectWithObject = delegate {  };
       
       public Func<Transform> onGetPlayerTransform = delegate { return null; };
       
       public UnityAction<bool> onCanPlayerInteractWithSomething = delegate {  };
       
       public UnityAction onTakeInteractableObject = delegate {  };
      
        
    }
}