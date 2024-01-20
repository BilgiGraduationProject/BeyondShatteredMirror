
using Runtime.Keys.Input;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Commands.Input
{
    public struct InputMovementCommand
    {
        private float _axisX, _axisZ;
        private readonly string _horizontal, _vertical;
      

        public InputMovementCommand(ref float axisX, ref float axisZ, string horizontal, string vertical)
        {
            _axisX = axisX;
            _axisZ = axisZ;
            _horizontal = horizontal;
            _vertical = vertical;
        }

        public void Execute( bool isReadyToMove, bool isInputReady)
        {
            if (isReadyToMove)
            {
                
                    if (UnityEngine.Input.GetButton(_vertical) || UnityEngine.Input.GetButton(_horizontal))
                    {
                        _axisX = UnityEngine.Input.GetAxisRaw(_horizontal);
                        _axisZ = UnityEngine.Input.GetAxisRaw(_vertical);
                        Debug.LogWarning("Movement");

                        InputSignals.Instance.onPlayerPressedMovementButton?.Invoke(new InputParams()
                        {
                            Horizontal = _axisX,
                            Vertical = _axisZ
                        });
                    }
                    else
                    {
                        InputSignals.Instance.onPlayerPressedMovementButton?.Invoke(new InputParams()
                        {
                            Horizontal = 0,
                            Vertical = 0
                        });
                        CoreGameSignals.Instance.onIsPlayerReadyToMove?.Invoke(false);
                    }
                
                
            }
            else
            {
                if (!isInputReady) return;
                if (UnityEngine.Input.GetButtonDown(_vertical) || UnityEngine.Input.GetButtonDown(_horizontal))
                {
                    CoreGameSignals.Instance.onIsPlayerReadyToMove?.Invoke(true);
                }

            }
            
                
            
            
            
            
            
           
        }
    }
}