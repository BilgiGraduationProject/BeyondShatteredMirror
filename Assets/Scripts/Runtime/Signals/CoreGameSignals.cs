using System;
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
        public Func<GameStateEnum> onGetGameState;
        public Func<PlayableEnum, Transform> onGetCameraCutScenePosition;
    }
}