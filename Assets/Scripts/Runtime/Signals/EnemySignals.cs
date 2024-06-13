using System;
using Runtime.Enums.Enemy;
using Runtime.Enums.Player;
using Runtime.Extentions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class EnemySignals : MonoSingleton<EnemySignals>
    {
        public UnityAction onEnemyDied = delegate {  };
        public UnityAction onResetEnemy = delegate {  };
       
        
        public UnityAction onStartHakanRun = delegate {  };
        
        public UnityAction<bool> onHakanIsAttacking = delegate {  };
        public UnityAction<bool> onHakanIsReadyToAttack = delegate {  };
        public UnityAction<bool> onHakanIsRoaring = delegate {  };
        public UnityAction<float> onSetHakanHealth = delegate {  };
        
        public UnityAction<bool> onIsTakingDamage = delegate {  };
        
        
        public UnityAction onSetSecondStageForHakan = delegate  {  };
        public UnityAction onFirstDieOfHakanForSlider = delegate {  };
        public UnityAction<bool> onHakanFirstDie = delegate {  };
        
        public UnityAction onSecondDieOfHakanForSlider = delegate {  };
        
        public UnityAction onDeathOfHakan = delegate {  };
        
        
        public UnityAction onResetHakan = delegate {  };
        
        
        public UnityAction onOpenHakanUI = delegate {  };
    }
}