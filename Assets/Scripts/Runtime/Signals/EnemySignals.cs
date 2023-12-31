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
   
        public UnityAction<EnemyAnimationState> onChangeEnemyAnimationState = delegate {  };
        public UnityAction<GameObject> onCheckEnemyHealth = delegate {  };
        public UnityAction<float> onGetEnemyHealth = delegate {  };
        public UnityAction<EnemyAnimationState,GameObject> onPlayerBodyCollidedWithEnemey = delegate {  };
        public UnityAction<Transform> onEnemyDied = delegate {  };
        public UnityAction<GameObject> onShowEnemyHealthBar = delegate {  };
        public UnityAction<GameObject> onHideEnemyHealthBar = delegate {  };
        
    }
}