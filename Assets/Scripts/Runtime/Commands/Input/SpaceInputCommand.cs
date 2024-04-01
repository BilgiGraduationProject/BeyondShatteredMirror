using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Input
{
    public struct SpaceInputCommand
    {
        private readonly string _space;
        public SpaceInputCommand(string space)
        {
            _space = space;
        }

        public void Execute( bool isCrouch,bool isInput)
        {
            if (isCrouch && !isInput) return;
            if (UnityEngine.Input.GetButtonDown(_space))
            {
                InputSignals.Instance.onPlayerPressedSpaceButton?.Invoke();
            }
        }
    }
}