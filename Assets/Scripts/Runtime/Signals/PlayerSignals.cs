using System;
using Runtime.Enums;
using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PlayerSignals : MonoSingleton<PlayerSignals>
    {
       public UnityAction<PlayableEnum> onSetPlayerToCutScenePosition = delegate {  };
       public UnityAction<PlayerAnimationState,bool> onSetAnimationBool = delegate {  };
       public UnityAction<PlayerAnimationState> onSetAnimationTrigger = delegate {  };
       public UnityAction<float> onSetCombatCount = delegate {  };
       public UnityAction<bool> onPlayerIsRolling = delegate {  };
       public UnityAction onPlayerReadyToKillTheEnemy = delegate {  };
       public UnityAction<float> onSetAnimationPlayerSpeed = delegate {  };
       public Func<PlayableEnum,Transform> onGetLevelCutScenePosition = delegate { return null; };
       public UnityAction<float> onSendPlayerSpeedToSlider = delegate {  };
       public UnityAction<float> onTakeDamage = delegate {  };
       public UnityAction<float> onHitDamage = delegate {  };
       public UnityAction<float> onSetHealthValue = delegate {  };
       public UnityAction<float> onSetHappinessValue = delegate {  };
       public UnityAction<float> onSetSensivity = delegate {  }; 
       public UnityAction<PillTypes> onSetPillEffect = delegate {  };
       
       public UnityAction onPlayerSaveTransform = delegate {  };
       public UnityAction onPlayerLoadTransform = delegate {  };
       
    }
}