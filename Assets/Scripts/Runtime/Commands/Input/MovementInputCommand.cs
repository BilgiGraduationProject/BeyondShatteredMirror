using Runtime.Keys.Input;
using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Input
{
    public struct MovementInputCommand
    {
        private float _verticalInput;
        private float _horizontalInput;
        private readonly string _horizontal;
        private readonly string _vertical;
       
        public MovementInputCommand(ref float verticalInput, ref float horizontalInput, string horizontal,
            string vertical)
        {
            _verticalInput = verticalInput;
            _horizontalInput = horizontalInput;
            _horizontal = horizontal;
            _vertical = vertical;
        }

        public void Execute( ref bool isInput, ref bool isButtonReady,bool isMovementInputReadyToUse )
        {
            if (!isMovementInputReadyToUse) return;
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
                    isButtonReady = false;
                    
                   InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(isInput);
                }
            }
            else
            {
                if(UnityEngine.Input.GetButton(_horizontal) || UnityEngine.Input.GetButton(_vertical))
                {
                    if (isButtonReady) return;
                    isInput = true;
                    InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(isInput);
                    isButtonReady = true;

                }
            }
            
            
        }
    }
}