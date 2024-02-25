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

        public void Execute()
        {
            if (UnityEngine.Input.GetButtonDown(_space))
            {
                InputSignals.Instance.onPlayerPressedSpaceButton?.Invoke();
            }
        }
    }
}