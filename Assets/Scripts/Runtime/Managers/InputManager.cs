
using System.Collections;
using DG.Tweening;
using Runtime.Commands.Input;
using Runtime.Keys.Input;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables




        [Header("Variables")] 
        [Header("Movement")]
        private float _verticalInput;
        private float _horizontalInput;
        private bool _isInput;
        private bool _isCrouch;
        private bool _isRun;
        [Header(("Cancel Movement"))] 
        private bool _isInputReadyToUse = true;


        [Header("Input Keys")] 
        private readonly string horizontal = "Horizontal";
        private readonly string vertical = "Vertical";
        private readonly string leftControl = "Left Control";
        private readonly string leftShift = "Left Shift";
        private readonly string space = "Space";


        [Header("Commands")]
        private MovementInputCommand _movementInputCommand;
        private CrouchInputCommand _crouchInputCommand;
        private RunInputCommand _runInputCommand;
        private SpaceInputCommand _spaceInputCommand;

        

        #endregion

        #endregion


        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _movementInputCommand = new MovementInputCommand(ref _verticalInput,ref _horizontalInput,  horizontal, vertical);
            _crouchInputCommand = new CrouchInputCommand(leftControl);
            _runInputCommand = new RunInputCommand(leftShift);
            _spaceInputCommand = new SpaceInputCommand(space);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onChangeMouseVisibility += OnChangeMouseVisibility;
            InputSignals.Instance.onIsInputReadyToUse += OnIsInputReadyToUse;
        }

        private void OnIsInputReadyToUse(bool condition)
        {
            _isInputReadyToUse = condition;
            InputSignals.Instance.onIsPlayerReadyToMove?.Invoke(condition);
           
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



        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onChangeMouseVisibility -= OnChangeMouseVisibility;
            InputSignals.Instance.onIsInputReadyToUse -= OnIsInputReadyToUse;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Update()
        {
            _runInputCommand.Execute(_isRun);
            if (!_isInputReadyToUse) return;
            _movementInputCommand.Execute(ref _isInput);
            _crouchInputCommand.Execute(ref _isCrouch);
            _spaceInputCommand.Execute();
            if (Input.GetMouseButtonDown(0))
            {
                InputSignals.Instance.onPlayerPressedRightMouseButton?.Invoke(true);
                

            }
           


                

        }

        
    }

       
    }


