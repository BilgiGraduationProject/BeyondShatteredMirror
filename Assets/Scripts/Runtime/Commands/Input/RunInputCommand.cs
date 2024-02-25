using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Input
{
    public struct RunInputCommand
    {
        private readonly string _leftShift;
        public RunInputCommand(string leftShift)
        {
            _leftShift = leftShift;
        }

        public void Execute()
        {
            if (UnityEngine.Input.GetButtonDown(_leftShift))
            {
                InputSignals.Instance.onPlayerPressedLeftShiftButton?.Invoke(true);
            }

            if (UnityEngine.Input.GetButtonUp(_leftShift))
            {
                InputSignals.Instance.onPlayerPressedLeftShiftButton?.Invoke(false);
            }
        }
    }
}