using System;
using System.Collections.Generic;
using UnityEngine.Playables;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct PlayerPlayableData
    {
        public List<PlayableAsset> playerPlayableAssets;
    }
}