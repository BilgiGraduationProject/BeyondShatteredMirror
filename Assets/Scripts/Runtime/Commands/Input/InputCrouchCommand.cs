using Runtime.Enums.GameManager;
using Runtime.Signals;

namespace Runtime.Commands.Input
{
    public readonly struct InputCrouchCommand
    {
        private readonly string _crouch;
        public InputCrouchCommand(string crouch) => _crouch = crouch;
        public void Execute()
        {
            if (!UnityEngine.Input.GetButtonDown(_crouch)) return;
            InputSignals.Instance.onPlayerPressedCrouchButton?.Invoke();
        }
    }
}