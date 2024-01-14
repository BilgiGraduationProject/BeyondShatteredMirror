
using Runtime.Commands.Input;
using Runtime.Enums.GameManager;
using Runtime.Signals;
using Sirenix.OdinInspector.Editor;
using UnityEngine;


namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables
        
        private InputMovementCommand _inputMovementCommand;
        private InputRunCommand _inputRunCommand;
        private InputCrouchCommand _inputCrouchCommand;
        private InputRollCommand _inputRollCommand;

        
        [Header("Variables")]
        private bool _isReadyToMove;
        private bool _isInputReady = true;
        private float _axisX, _axisZ;
        private bool _isRoll = true;
        
        
        [Header("Input Keys")]
        private readonly string _horizontal = "Horizontal";
        private readonly string _vertical = "Vertical";
        private readonly string _leftShift = "Left Shift";
        private readonly string _roll = "Roll";
        private readonly string _crouch = "Left Control";
        private readonly string _attack = "Attack";
        private readonly string _mouseRightButton = "Counter";
        private readonly string _pistol = "Pistol";
        private readonly string _punch = "Punch";

        #endregion
        
        #endregion

        private void Awake()
        {
            Init();
            
            
        }
        private void Init()
        {
            _inputMovementCommand = new InputMovementCommand(ref _axisX,ref _axisZ, _horizontal, _vertical);
            _inputRunCommand = new InputRunCommand(_leftShift);
            _inputCrouchCommand = new InputCrouchCommand(_crouch);
            _inputRollCommand = new InputRollCommand(_roll);
        }
        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onIsPlayerReadyToMove += OnIsPlayerReadyToMove;
            CoreGameSignals.Instance.onIsInputReady += OnIsInputReady;
            InputSignals.Instance.onPlayerIsAvailableForRoll += OnIsAvailebleForRoll; 
            InputSignals.Instance.onChangeMouseVisibility += OnChangeMouseVisibility;
            
            
        }

        private void OnChangeMouseVisibility(bool condition)
        {
            if (condition)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }


        private void OnIsAvailebleForRoll(bool condition) => _isRoll = condition;
       

        private void OnIsInputReady(bool condition)
        {
            _isInputReady = condition;
            CoreGameSignals.Instance.onIsPlayerReadyToMove?.Invoke(condition);
        }

        private void OnIsPlayerReadyToMove(bool condition) => _isReadyToMove = condition;
        
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onIsPlayerReadyToMove -= OnIsPlayerReadyToMove;
            CoreGameSignals.Instance.onIsInputReady -= OnIsInputReady;
            InputSignals.Instance.onChangeMouseVisibility -= OnChangeMouseVisibility;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Update()
        {
           _inputMovementCommand.Execute(_isReadyToMove,_isInputReady);
           _inputRunCommand.Execute();
           _inputCrouchCommand.Execute();
           _inputRollCommand.Execute(_isRoll);

           if (Input.GetButtonDown(_attack))
           {
              InputSignals.Instance.onPlayerPressedAttackButton?.Invoke();
           }

           if (Input.GetButtonDown(_mouseRightButton))
           {
               InputSignals.Instance.onPlayerPressedMouseButtonRight?.Invoke();
           }

           if (Input.GetButtonUp(_mouseRightButton))
           {
               InputSignals.Instance.onPlayerReleasedMouseButtonRight?.Invoke();
           }

           if (Input.GetButtonDown(_pistol))
           {
               CoreGameSignals.Instance.onChangeGameFightState?.Invoke(GameFightStateEnum.Pistol);
           }

           if (Input.GetButtonDown(_punch))
           {
               CoreGameSignals.Instance.onChangeGameFightState?.Invoke(GameFightStateEnum.Punch);
           }
           
            
        }

        

        
    }
}
