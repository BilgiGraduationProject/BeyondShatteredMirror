using System;
using System.Collections.Generic;
using Runtime.Enums.Enemy;
using Runtime.Enums.GameManager;
using Runtime.Enums.Playable;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        public UnityAction<GameStateEnum> onGameStatusChanged = delegate {  };
        public Func<GameStateEnum> onGetGameState = delegate { return GameStateEnum.Game; };
        public Func<PlayableEnum, Transform> onGetCameraCutScenePosition = delegate { return null; };
        public UnityAction<PlayableEnum> onGameManagerGetCurrentGameState = delegate {  };
        public Func<PlayableEnum> onSendCurrentGameStateToUIText = () => PlayableEnum.BathroomLayingSeize;
        
        
        
        
        public UnityAction<Transform,Transform,Transform,Transform> onGetCheckPointsList = delegate { };
    }
}