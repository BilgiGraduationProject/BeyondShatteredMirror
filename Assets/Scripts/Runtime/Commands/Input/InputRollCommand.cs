using Runtime.Enums.GameManager;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Input
{
    public struct InputRollCommand
    {
        private readonly string _roll;
        public InputRollCommand(string roll) => _roll = roll;
        public void Execute(bool isRoll)
        {
            if (UnityEngine.Input.GetButtonDown(_roll) && isRoll)
            {
                InputSignals.Instance.onPlayerPressedRollButton?.Invoke();
                InputSignals.Instance.onPlayerIsAvailableForRoll?.Invoke(false);
            }
            
        }
    }
}