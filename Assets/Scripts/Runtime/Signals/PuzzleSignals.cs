using System;
using Runtime.Extentions;
using Runtime.Keys.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class PuzzleSignals : MonoSingleton<PuzzleSignals>
    {
        public UnityAction<GameObject,bool> onChangePuzzleColor = delegate {  };
        public UnityAction<GameObject,GameObject> onInteractWithPuzzlePieces = delegate {  };
        public Func<int> onGetPuzzleCatEyeValues = delegate { return 0;};
        
    }
}