using Runtime.Signals;

namespace Runtime.Commands.Input
{
    public struct InputRunCommand
    {
        private readonly string _leftShift;
        public InputRunCommand(string leftShift)
        {
            _leftShift = leftShift;
        }

        public void Execute()
        {
            if (UnityEngine.Input.GetButtonDown(_leftShift))
            {
                InputSignals.Instance.onPlayerPressedRunButton?.Invoke();
            }

            if (UnityEngine.Input.GetButtonUp(_leftShift))
            {
                InputSignals.Instance.onPlayerReleaseRunButton?.Invoke();   
            }
        }
    }
}