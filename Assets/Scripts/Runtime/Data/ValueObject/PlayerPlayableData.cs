using System;
using System.Collections.Generic;
using Runtime.Enums.Playable;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PlayerPlayableData
    {
        public PlayableEnum playableEnum;
        public PlayableAsset playerPlayableAssets;
        public DirectorWrapMode directorWrapMode;
    }
}