using System;
using Runtime.Enums.Collectable;
using Runtime.Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class InteractableSignals : MonoSingleton<InteractableSignals>
    {
        public UnityAction<bool,GameObject> onChangeColorOfInteractableObject = delegate {  };
        public UnityAction<GameObject,Transform> onPickUpTheInteractableObject = delegate {  };
        public UnityAction<GameObject,Transform> onDropTheInteractableObject = delegate {  };
        public UnityAction<GameObject,GameObject,Transform> onDropandPickUpTheInteractableObject = delegate {  };
        public UnityAction<GameObject> onInteractableOpenDoor = delegate {  };
        
       
    }
}