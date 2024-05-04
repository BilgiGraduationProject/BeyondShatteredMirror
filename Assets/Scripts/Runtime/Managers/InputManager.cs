
using System;
using System.Collections;
using DG.Tweening;
using Runtime.Commands.Input;
using Runtime.Enums.Playable;
using Runtime.Enums.Player;
using Runtime.Enums.UI;
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
        private bool _isCanTouchButton;
       
        private PlayableEnum _playableEnumIndex;
        [Header(("Cancel Movement"))] 
        private bool _isMovementInputIsReadyToUse = true;
        [Header(("Combat"))]
        private int _combatCount;
        private Coroutine _combatCoroutine;
        private bool _isCombat;
        [Header("Actions")]
        
        private bool _isCrouch;
        private bool _isRun;
        private bool _isCutSceneInputReadyToUse;
        private bool _isPickingUp;
        


        [Header("Input Keys")] 
        private readonly string horizontal = "Horizontal";
        private readonly string vertical = "Vertical";
        private readonly string leftControl = "Left Control";
        private readonly string leftShift = "Left Shift";
        private readonly string space = "Space";
        private readonly string leftMouseButton = "LeftMouseButton";


        [Header("Commands")]
        private MovementInputCommand _movementInputCommand;
        private CrouchInputCommand _crouchInputCommand;
        private RunInputCommand _runInputCommand;
        private SpaceInputCommand _spaceInputCommand;
        private MeeleCombatCommand _meeleCombatCommand;

        

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
            _meeleCombatCommand = new MeeleCombatCommand(leftMouseButton,this,ref _combatCoroutine);
            
        }

        private void Start()
        {
            //OnChangeMouseVisibility(false);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onChangeMouseVisibility += OnChangeMouseVisibility;
            InputSignals.Instance.onIsMovementInputReadyToUse += OnIsInputReadyToUse;
            InputSignals.Instance.onIsReadyForCombat += OnIsReadyForCombat;
            PlayableSignals.Instance.onSendInputManagerToReadyForInput += OnSendInputManagerToReadyForInput;
        }

        private void OnIsReadyForCombat(bool condition) => _isCombat = condition;
        
        
        private void OnIsPlayerPickingItem(bool condition)
        {
            _isPickingUp = condition;
        }
        

        private void OnSendInputManagerToReadyForInput(bool condition, PlayableEnum playableEnum)
        {
            _isCutSceneInputReadyToUse = condition;
            _playableEnumIndex = playableEnum;
        }

        private void OnIsInputReadyToUse(bool condition)
        {
            
            _isMovementInputIsReadyToUse = condition;
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
            InputSignals.Instance.onIsMovementInputReadyToUse -= OnIsInputReadyToUse;
            InputSignals.Instance.onIsReadyForCombat -= OnIsReadyForCombat;
            InputSignals.Instance.onIsPlayerPickingItem -= OnIsPlayerPickingItem;
            PlayableSignals.Instance.onSendInputManagerToReadyForInput -= OnSendInputManagerToReadyForInput;
        }

       

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Update()
        {
            _runInputCommand.Execute(_isRun);
            if (_isCutSceneInputReadyToUse)
            {
                if (Input.GetButtonDown(horizontal) || Input.GetButtonDown(vertical))
                {
                    switch (_playableEnumIndex)
                    {
                        case PlayableEnum.BathroomLayingSeize:
                            UITextSignals.Instance.onChangeMissionText?.Invoke(UITextEnum.GoToMirror);
                            PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.StandUp);
                            _isCutSceneInputReadyToUse = false;
                            break;
                        case PlayableEnum.EnteredHouse:
                            PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.StandUp);
                            _isCutSceneInputReadyToUse = false;
                            break;
                        
                    }
                }
                    
            }

            // They have _isInputReady unless they are true.
            _movementInputCommand.Execute(ref _isInput,ref _isCanTouchButton,_isMovementInputIsReadyToUse);
            _crouchInputCommand.Execute(ref _isCrouch,_isMovementInputIsReadyToUse);
            _spaceInputCommand.Execute(_isCrouch,_isMovementInputIsReadyToUse);
            // -------------------------------------------------
            
            _meeleCombatCommand.Execute(ref _combatCount,_isCombat,_isMovementInputIsReadyToUse);

            if (Input.GetKeyDown(KeyCode.E) && !_isPickingUp)
            {
                InputSignals.Instance.onPlayerPressedPickUpButton?.Invoke();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                InputSignals.Instance.onPlayerPressedDropItemButton?.Invoke();
            }
            




        }

        internal IEnumerator CancelPlayerCombat()
        {
            
            yield return new WaitForSeconds(3f);
            _combatCount = 0;
            yield return new WaitForSeconds(10f);
            PlayerSignals.Instance.onSetAnimationBool?.Invoke(PlayerAnimationState.Combat, false);
        }
    }

       
    }


