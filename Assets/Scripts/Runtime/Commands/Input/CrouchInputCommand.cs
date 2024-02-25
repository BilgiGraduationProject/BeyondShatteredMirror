using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Input
{
    public struct CrouchInputCommand
    {
        private readonly string _leftControl;
        public CrouchInputCommand(string leftControl)
        {
            _leftControl = leftControl;
        }

        public void Execute(ref bool isCrouch)
        {
            if (!UnityEngine.Input.GetButtonDown(_leftControl)) return;
            isCrouch = !isCrouch;
            InputSignals.Instance.onPlayerPressedLeftControlButton.Invoke(isCrouch);
            
        }
    }
}