using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PlayerPlayableData
    {
        public PlayableAsset playerPlayableAssets;
        public DirectorWrapMode directorWrapMode;
    }
}