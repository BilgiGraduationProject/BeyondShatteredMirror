using Runtime.Keys.Input;
using Runtime.Managers;
using Runtime.Signals;

namespace Runtime.Commands.Input
{
    public struct MovementInputCommand
    {
        private float _verticalInput;
        private float _horizontalInput;
        private readonly string _horizontal;
        private readonly string _vertical;
        public MovementInputCommand(ref float verticalInput, ref float horizontalInput, string horizontal, string vertical)
        {
            _verticalInput = verticalInput;
            _horizontalInput = horizontalInput;
            _horizontal = horizontal;
            _vertical = vertical;
        }

        public void Execute(ref bool isInput)
        {
            if (isInput)
            {
                
                if(UnityEngine.Input.GetButton(_horizontal) || UnityEngine.Input.GetButton(_vertical))
                {
                    _horizontalInput = UnityEngine.Input.GetAxisRaw(_horizontal);
                    _verticalInput = UnityEngine.Input.GetAxisRaw(_vertical);
              
                    InputSignals.Instance.onSendInputParams?.Invoke(new InputParams()
                    {
                        Horizontal = _horizontalInput,
                        Vertical = _verticalInput
                    });
                }
                else
                {
                    
                    InputSignals.Instance.onSendInputParams?.Invoke(new InputParams()
                    {
                        Horizontal = 0,
                        Vertical = 0,
                    });
                    isInput = false;
                    
                   InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(isInput);
                }
            }
            
            if(UnityEngine.Input.GetButtonDown(_horizontal) || UnityEngine.Input.GetButtonDown(_vertical))
            {
                isInput = true;
                InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(isInput);
                
            }
        }
    }
}